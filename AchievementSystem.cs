using System;
using System.Collections.Generic;
using UnityEngine;
using Steamworks;
using Org.Yoobingo;

public enum AchievementID
{
    /// <summary>完成第一天的营业 - Complete your very first business day.</summary>
    COMPLETE_DAY_ONE,

    /// <summary>第一次扩建 - Expand your shop for the first time.</summary>
    FIRST_EXPANSION,

    /// <summary>第一次培育出小猫 - Breed your very first kitten.</summary>
    FIRST_KITTEN_BRED,

    /// <summary>收银 100 次 - Complete 100 successful checkouts.</summary>
    CHECKOUT_100_TIMES,

    /// <summary>第一次雇佣猫咪员工 - Hire your first feline employee.</summary>
    HIRE_FIRST_CAT_EMPLOYEE,

    /// <summary>家猫毛色全收集 - Collect every color variant of the domestic cat.</summary>
    COLLECT_ALL_DOMESTIC_CAT_COLORS,

    /// <summary>营业 100 天 - Keep the shop running for 100 business days.</summary>
    OPERATE_100_DAYS,

    /// <summary>解锁所有扩建 - Unlock every shop expansion.</summary>
    UNLOCK_ALL_EXPANSIONS,

    /// <summary>收集 5 种猫咪的全部毛色 - Complete every color collection for five cat breeds.</summary>
    COLLECT_FIVE_BREEDS_ALL_COLORS,
}

/// <summary>
/// Steam Stat API Name 枚举。
/// 如果某个成就不需要进度，使用 None。
/// 其他枚举名必须和 Steam 后台 Stat API Name 完全一致。
/// </summary>
public enum AchievementStatID
{
    None,

    /// <summary>收银次数</summary>
    STAT_CHECKOUT_COUNT,

    /// <summary>营业天数</summary>
    STAT_OPERATE_DAYS,

    /// <summary>家猫已收集毛色数量</summary>
    STAT_DOMESTIC_CAT_COLOR_COUNT,

    /// <summary>已解锁扩建数量</summary>
    STAT_EXPANSION_UNLOCK_COUNT,

    /// <summary>已完成全毛色收集的猫咪品种数量</summary>
    STAT_BREED_FULL_COLOR_COLLECTION_COUNT,
}

public class AchievementSystem : MonoBehaviour
{
    [Serializable]
    public class AchievementTask
    {
        [Header("Steam Achievement")]
        [Tooltip("枚举名必须和 Steam 后台 Achievement API Name 完全一致")]
        public AchievementID achievementID;

        [Header("Steam Stat / Progress")]
        [Tooltip("一次性成就选 None；进度成就选择对应 Steam Stat API Name")]
        public AchievementStatID statID = AchievementStatID.None;

        [Tooltip("达到多少进度后解锁。一次性成就填 1")]
        public int targetProgress = 1;

        [Tooltip("进度变化时是否允许弹出 Steam 进度提示")]
        public bool allowProgressPopup = true;

        [NonSerialized] public int currentProgress;
        [NonSerialized] public bool unlocked;

        public string AchievementApiName => achievementID.ToString();

        public bool HasProgressStat => statID != AchievementStatID.None;

        public string StatApiName => statID.ToString();

        public int SafeTarget => Mathf.Max(1, targetProgress);
    }

    public static AchievementSystem Instance { get; private set; }

    /// <summary>Steam 当前用户 Stats / Achievements 拉取成功后触发。</summary>
    public event Action OnStatsReady;

    /// <summary>Steam 当前用户 Stats / Achievements 拉取失败后触发。</summary>
    public event Action<EResult> OnStatsLoadFailed;

    /// <summary>本地成就进度变化后触发。参数：成就 ID、当前进度、目标进度。</summary>
    public event Action<AchievementID, int, int> OnProgressChanged;

    /// <summary>本地成功调用 SetAchievement 后触发，此时还不代表 Steam 服务器已经确认写入。</summary>
    public event Action<AchievementID> OnAchievementUnlockedLocal;

    /// <summary>Steam 通过 UserAchievementStored_t 确认成就写入后触发。</summary>
    public event Action<AchievementID> OnAchievementStoredSteam;

    /// <summary>StoreStats 上传成功后触发。</summary>
    public event Action<EResult> OnStatsStoredSuccess;

    /// <summary>StoreStats 上传失败后触发。</summary>
    public event Action<EResult> OnStatsStoredFailed;

    [Header("成就任务配置")]
    [SerializeField]
    private AchievementTask[] achievementTasks =
    {
        new AchievementTask
        {
            achievementID = AchievementID.COMPLETE_DAY_ONE,
            statID = AchievementStatID.None,
            targetProgress = 1,
            allowProgressPopup = false
        },

        new AchievementTask
        {
            achievementID = AchievementID.FIRST_EXPANSION,
            statID = AchievementStatID.None,
            targetProgress = 1,
            allowProgressPopup = false
        },

        new AchievementTask
        {
            achievementID = AchievementID.FIRST_KITTEN_BRED,
            statID = AchievementStatID.None,
            targetProgress = 1,
            allowProgressPopup = false
        },

        new AchievementTask
        {
            achievementID = AchievementID.CHECKOUT_100_TIMES,
            statID = AchievementStatID.STAT_CHECKOUT_COUNT,
            targetProgress = 100,
            allowProgressPopup = true
        },

        new AchievementTask
        {
            achievementID = AchievementID.HIRE_FIRST_CAT_EMPLOYEE,
            statID = AchievementStatID.None,
            targetProgress = 1,
            allowProgressPopup = false
        },

        new AchievementTask
        {
            achievementID = AchievementID.COLLECT_ALL_DOMESTIC_CAT_COLORS,
            statID = AchievementStatID.STAT_DOMESTIC_CAT_COLOR_COUNT,
            targetProgress = 1,
            allowProgressPopup = true
        },

        new AchievementTask
        {
            achievementID = AchievementID.OPERATE_100_DAYS,
            statID = AchievementStatID.STAT_OPERATE_DAYS,
            targetProgress = 100,
            allowProgressPopup = true
        },

        new AchievementTask
        {
            achievementID = AchievementID.UNLOCK_ALL_EXPANSIONS,
            statID = AchievementStatID.STAT_EXPANSION_UNLOCK_COUNT,
            targetProgress = 1,
            allowProgressPopup = true
        },

        new AchievementTask
        {
            achievementID = AchievementID.COLLECT_FIVE_BREEDS_ALL_COLORS,
            statID = AchievementStatID.STAT_BREED_FULL_COLOR_COLLECTION_COUNT,
            targetProgress = 5,
            allowProgressPopup = true
        },
    };

    [Header("提交设置")]
    [Tooltip("普通进度变化后，自动提交到 Steam 的间隔。不要高频 StoreStats")]
    [SerializeField] private float autoStoreInterval = 60f;

    private readonly Dictionary<AchievementID, AchievementTask> taskMap =
        new Dictionary<AchievementID, AchievementTask>();

    private CGameID gameID;

    private bool callbacksRegistered;
    private bool statsReady;
    private bool dirty;
    private bool storeInFlight;

    private int dirtyVersion;
    private int storeRequestVersion;

    private float nextAutoStoreTime;

    private Callback<UserStatsReceived_t> userStatsReceivedCallback;
    private Callback<UserStatsStored_t> userStatsStoredCallback;
    private Callback<UserAchievementStored_t> userAchievementStoredCallback;

    public bool StatsReady => statsReady;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        BuildTaskMap();
    }

    public void StartSteamAchievementSystem()
    {
        TryRegisterCallbacksAndRequestStats();
    }

    private void Update()
    {
        TryRegisterCallbacksAndRequestStats();

        if (!SteamManager.Initialized)
            return;

        if (!statsReady)
            return;

        if (dirty && !storeInFlight && Time.unscaledTime >= nextAutoStoreTime)
        {
            FlushStats();
        }
    }

    private void OnApplicationQuit()
    {
        if (SteamManager.Initialized && statsReady && dirty && !storeInFlight)
        {
            FlushStats();
        }
    }

    private void BuildTaskMap()
    {
        taskMap.Clear();

        foreach (AchievementTask task in achievementTasks)
        {
            if (task == null)
                continue;

            if (taskMap.ContainsKey(task.achievementID))
            {
                Debug.LogWarning($"[AchievementSystem] Duplicate achievement config: {task.achievementID}");
                continue;
            }

            taskMap.Add(task.achievementID, task);
        }
    }

    private void TryRegisterCallbacksAndRequestStats()
    {
        if (callbacksRegistered)
            return;

        if (!SteamManager.Initialized)
            return;

        callbacksRegistered = true;
        gameID = new CGameID(SteamUtils.GetAppID());

        userStatsReceivedCallback =
            Callback<UserStatsReceived_t>.Create(OnUserStatsReceived);

        userStatsStoredCallback =
            Callback<UserStatsStored_t>.Create(OnUserStatsStored);

        userAchievementStoredCallback =
            Callback<UserAchievementStored_t>.Create(OnUserAchievementStored);

        RequestLatestStats();
    }

    /// <summary>
    /// 登录 / 启动后拉取 Steam 上当前玩家的最新成就和进度。
    /// </summary>
    public bool RequestLatestStats()
    {
        if (!SteamManager.Initialized)
        {
            Debug.LogWarning("[AchievementSystem] Steam is not initialized.");
            return false;
        }

        if (!SteamUser.BLoggedOn())
        {
            Debug.LogWarning("[AchievementSystem] Steam user is not logged on.");
            return false;
        }

        statsReady = false;

        bool success = SteamUserStats.RequestCurrentStats();

        if (!success)
        {
            Debug.LogWarning("[AchievementSystem] RequestCurrentStats failed.");
            OnStatsLoadFailed?.Invoke(EResult.k_EResultFail);
        }

        return success;
    }

    private void OnUserStatsReceived(UserStatsReceived_t callback)
    {
        if ((ulong)gameID != callback.m_nGameID)
            return;

        if (callback.m_eResult != EResult.k_EResultOK)
        {
            Debug.LogWarning($"[AchievementSystem] UserStatsReceived failed: {callback.m_eResult}");
            OnStatsLoadFailed?.Invoke(callback.m_eResult);
            return;
        }

        statsReady = true;

        foreach (AchievementTask task in achievementTasks)
        {
            if (task == null)
                continue;

            LoadAchievementFromSteam(task);
        }

        Debug.Log("[AchievementSystem] Steam stats and achievements loaded.");
        OnStatsReady?.Invoke();
    }

    private void LoadAchievementFromSteam(AchievementTask task)
    {
        string achievementApiName = task.AchievementApiName;

        bool getAchievementOk =
            SteamUserStats.GetAchievement(achievementApiName, out task.unlocked);

        if (!getAchievementOk)
        {
            Debug.LogWarning(
                $"[AchievementSystem] GetAchievement failed: {achievementApiName}. " +
                "Check Steamworks Achievement API Name and publish status.");
        }

        if (task.HasProgressStat)
        {
            string statApiName = task.StatApiName;

            bool getStatOk =
                SteamUserStats.GetStat(statApiName, out task.currentProgress);

            if (!getStatOk)
            {
                Debug.LogWarning(
                    $"[AchievementSystem] GetStat failed: {statApiName}. " +
                    "Check Steamworks Stat API Name, type and publish status.");
            }
        }
        else
        {
            task.currentProgress = task.unlocked ? 1 : 0;
        }

        Debug.Log(
            $"[AchievementSystem] Loaded {achievementApiName}: " +
            $"unlocked={task.unlocked}, progress={task.currentProgress}/{task.SafeTarget}");
    }

    /// <summary>
    /// 查询成就是否已解锁。
    /// </summary>
    public bool TryGetAchievementUnlocked(AchievementID achievementID, out bool unlocked)
    {
        unlocked = false;

        if (!EnsureReady())
            return false;

        string apiName = achievementID.ToString();

        bool success = SteamUserStats.GetAchievement(apiName, out unlocked);

        if (success && taskMap.TryGetValue(achievementID, out AchievementTask task))
        {
            task.unlocked = unlocked;

            if (!task.HasProgressStat)
                task.currentProgress = unlocked ? 1 : 0;
        }

        return success;
    }

    /// <summary>
    /// 获取本地缓存的进度。
    /// 需要先等待 StatsReady 为 true。
    /// </summary>
    public int GetProgress(AchievementID achievementID)
    {
        if (!taskMap.TryGetValue(achievementID, out AchievementTask task))
        {
            Debug.LogWarning($"[AchievementSystem] Unknown achievement: {achievementID}");
            return 0;
        }

        return task.currentProgress;
    }

    /// <summary>
    /// 获取目标进度。
    /// </summary>
    public int GetTargetProgress(AchievementID achievementID)
    {
        if (!taskMap.TryGetValue(achievementID, out AchievementTask task))
        {
            Debug.LogWarning($"[AchievementSystem] Unknown achievement: {achievementID}");
            return 1;
        }

        return task.SafeTarget;
    }

    /// <summary>
    /// 增加进度。
    /// 例如：AddProgress(AchievementID.CHECKOUT_100_TIMES, 1, true);
    /// </summary>
    public bool AddProgress(
        AchievementID achievementID,
        int amount = 1,
        bool storeNow = false,
        bool showSteamProgressPopup = true)
    {
        if (!EnsureReady())
            return false;

        if (!taskMap.TryGetValue(achievementID, out AchievementTask task))
        {
            Debug.LogWarning($"[AchievementSystem] Unknown achievement: {achievementID}");
            return false;
        }

        if (task.unlocked)
            return true;

        amount = Mathf.Max(0, amount);

        if (amount <= 0)
            return true;

        if (!task.HasProgressStat)
        {
            return UnlockAchievement(achievementID);
        }

        int oldProgress = task.currentProgress;
        int newProgress = Mathf.Clamp(oldProgress + amount, 0, task.SafeTarget);

        if (newProgress == oldProgress)
            return true;

        return SetProgressInternal(
            task,
            newProgress,
            storeNow,
            showSteamProgressPopup);
    }

    /// <summary>
    /// 设置绝对进度。
    /// 适合结算时直接同步总数，例如当前总收银次数、当前营业天数等。
    /// </summary>
    public bool SetProgressAbsolute(
        AchievementID achievementID,
        int progress,
        bool storeNow = false,
        bool showSteamProgressPopup = false)
    {
        if (!EnsureReady())
            return false;

        if (!taskMap.TryGetValue(achievementID, out AchievementTask task))
        {
            Debug.LogWarning($"[AchievementSystem] Unknown achievement: {achievementID}");
            return false;
        }

        if (task.unlocked)
            return true;

        progress = Mathf.Clamp(progress, 0, task.SafeTarget);

        if (!task.HasProgressStat)
        {
            if (progress >= task.SafeTarget)
                return UnlockAchievement(achievementID);

            return true;
        }

        return SetProgressInternal(
            task,
            progress,
            storeNow,
            showSteamProgressPopup);
    }

    private bool SetProgressInternal(
        AchievementTask task,
        int newProgress,
        bool storeNow,
        bool showSteamProgressPopup)
    {
        string statApiName = task.StatApiName;

        bool setStatOk =
            SteamUserStats.SetStat(statApiName, newProgress);

        if (!setStatOk)
        {
            Debug.LogWarning($"[AchievementSystem] SetStat failed: {statApiName}");
            return false;
        }

        task.currentProgress = newProgress;
        MarkDirty();

        OnProgressChanged?.Invoke(
            task.achievementID,
            task.currentProgress,
            task.SafeTarget);

        if (showSteamProgressPopup &&
            task.allowProgressPopup &&
            task.currentProgress > 0 &&
            task.currentProgress < task.SafeTarget)
        {
            SteamUserStats.IndicateAchievementProgress(
                task.AchievementApiName,
                (uint)task.currentProgress,
                (uint)task.SafeTarget);
        }

        if (task.currentProgress >= task.SafeTarget)
        {
            return UnlockAchievement(task.achievementID);
        }

        if (storeNow)
        {
            FlushStats();
        }

        return true;
    }

    /// <summary>
    /// 直接解锁成就。
    /// 适合一次性成就，例如完成第一天营业、第一次扩建、第一次雇佣猫咪员工等。
    /// </summary>
    public bool UnlockAchievement(AchievementID achievementID, bool storeNow = true)
    {
        if (!EnsureReady())
            return false;

        string apiName = achievementID.ToString();

        bool getAchievementOk =
            SteamUserStats.GetAchievement(apiName, out bool alreadyUnlocked);

        if (!getAchievementOk)
        {
            Debug.LogWarning($"[AchievementSystem] GetAchievement failed: {apiName}");
            return false;
        }

        if (alreadyUnlocked)
        {
            if (taskMap.TryGetValue(achievementID, out AchievementTask cachedTask))
            {
                cachedTask.unlocked = true;

                if (!cachedTask.HasProgressStat)
                    cachedTask.currentProgress = 1;
            }

            return true;
        }

        bool setAchievementOk =
            SteamUserStats.SetAchievement(apiName);

        if (!setAchievementOk)
        {
            Debug.LogWarning($"[AchievementSystem] SetAchievement failed: {apiName}");
            return false;
        }

        if (taskMap.TryGetValue(achievementID, out AchievementTask task))
        {
            task.unlocked = true;
            task.currentProgress = task.SafeTarget;

            if (task.HasProgressStat)
            {
                SteamUserStats.SetStat(task.StatApiName, task.SafeTarget);
            }

            OnProgressChanged?.Invoke(
                achievementID,
                task.currentProgress,
                task.SafeTarget);
        }

        MarkDirty();
        OnAchievementUnlockedLocal?.Invoke(achievementID);

        if (storeNow)
        {
            FlushStats();
        }

        Debug.Log($"[AchievementSystem] Achievement unlocked: {apiName}");

        return true;
    }

    /// <summary>
    /// 手动提交修改到 Steam。
    /// 建议在一局结束、一天营业结束、回主菜单、退出游戏、解锁成就后调用。
    /// </summary>
    public bool FlushStats()
    {
        if (!EnsureReady())
            return false;

        if (!dirty)
            return true;

        if (storeInFlight)
            return false;

        storeInFlight = true;
        storeRequestVersion = dirtyVersion;

        bool success = SteamUserStats.StoreStats();

        if (!success)
        {
            storeInFlight = false;
            nextAutoStoreTime = Time.unscaledTime + 10f;

            Debug.LogWarning("[AchievementSystem] StoreStats failed, will retry later.");
            OnStatsStoredFailed?.Invoke(EResult.k_EResultFail);
        }

        return success;
    }

    private void OnUserStatsStored(UserStatsStored_t callback)
    {
        if ((ulong)gameID != callback.m_nGameID)
            return;

        storeInFlight = false;

        if (callback.m_eResult == EResult.k_EResultOK)
        {
            if (storeRequestVersion == dirtyVersion)
            {
                dirty = false;
                nextAutoStoreTime = 0f;
            }
            else
            {
                nextAutoStoreTime = Time.unscaledTime + 5f;
            }

            Debug.Log("[AchievementSystem] StoreStats success.");
            OnStatsStoredSuccess?.Invoke(callback.m_eResult);
        }
        else if (callback.m_eResult == EResult.k_EResultInvalidParam)
        {
            Debug.LogWarning(
                "[AchievementSystem] StoreStats invalid param. " +
                "Reloading latest stats from Steam.");

            OnStatsStoredFailed?.Invoke(callback.m_eResult);
            RequestLatestStats();
        }
        else
        {
            Debug.LogWarning($"[AchievementSystem] StoreStats failed: {callback.m_eResult}");
            nextAutoStoreTime = Time.unscaledTime + 10f;
            OnStatsStoredFailed?.Invoke(callback.m_eResult);
        }
    }

    private void OnUserAchievementStored(UserAchievementStored_t callback)
    {
        if ((ulong)gameID != callback.m_nGameID)
            return;

        string achievementName = callback.m_rgchAchievementName;
        bool parsed = Enum.TryParse(achievementName, out AchievementID achievementID);

        if (callback.m_nCurProgress == 0 && callback.m_nMaxProgress == 0)
        {
            Debug.Log($"[AchievementSystem] Steam achievement stored: {achievementName}");

            if (parsed)
            {
                OnAchievementStoredSteam?.Invoke(achievementID);
            }
        }
        else
        {
            Debug.Log(
                $"[AchievementSystem] Steam achievement progress: " +
                $"{achievementName} {callback.m_nCurProgress}/{callback.m_nMaxProgress}");

            if (parsed)
            {
                OnProgressChanged?.Invoke(
                    achievementID,
                    (int)callback.m_nCurProgress,
                    (int)callback.m_nMaxProgress);
            }
        }
    }

    private void MarkDirty()
    {
        dirty = true;
        dirtyVersion++;

        if (nextAutoStoreTime <= 0f || Time.unscaledTime >= nextAutoStoreTime)
        {
            nextAutoStoreTime = Time.unscaledTime + autoStoreInterval;
        }
    }

    private bool EnsureReady()
    {
        if (!SteamManager.Initialized)
        {
            Debug.LogWarning("[AchievementSystem] Steam is not initialized.");
            return false;
        }

        if (!statsReady)
        {
            Debug.LogWarning(
                "[AchievementSystem] Stats are not ready yet. " +
                "Wait for RequestLatestStats callback.");
            return false;
        }

        return true;
    }

#if UNITY_EDITOR || DEVELOPMENT_BUILD
    private bool _guiVisible = true;
    private Vector2 _guiScroll;

    private void OnGUI()
    {
        if (!statsReady)
        {
            GUI.Label(new Rect(10, 10, 300, 25), "[AchievementSystem] Stats not ready...");
            return;
        }

        // 折叠按钮
        if (GUI.Button(new Rect(10, 10, 180, 25), _guiVisible ? "▼ Achievement Debug" : "▶ Achievement Debug"))
            _guiVisible = !_guiVisible;

        if (!_guiVisible) return;

        float panelX = 10f;
        float panelY = 40f;
        float panelW = 420f;
        float rowH   = 28f;
        float panelH = Mathf.Min(achievementTasks.Length * rowH + 40f, 500f);

        GUI.Box(new Rect(panelX, panelY, panelW, panelH), "");

        // 重置按钮
        if (GUI.Button(new Rect(panelX + 4, panelY + 4, 120, 22), "Reset All (Test)"))
            ResetAllForTest(true);

        float scrollTop = panelY + 30f;
        float scrollH   = panelH - 34f;
        float contentH  = achievementTasks.Length * rowH;

        _guiScroll = GUI.BeginScrollView(
            new Rect(panelX + 2, scrollTop, panelW - 4, scrollH),
            _guiScroll,
            new Rect(0, 0, panelW - 20, contentH));

        for (int i = 0; i < achievementTasks.Length; i++)
        {
            AchievementTask task = achievementTasks[i];
            if (task == null) continue;

            float y = i * rowH;

            // 进度标签
            string label = task.unlocked
                ? $"✔ {task.achievementID}"
                : $"{task.achievementID}  {task.currentProgress}/{task.SafeTarget}";

            GUI.Label(new Rect(0, y + 4, 280, 22), label);

            if (!task.unlocked)
            {
                // +1 进度按钮
                if (GUI.Button(new Rect(284, y + 2, 50, 22), "+1"))
                    AddProgress(task.achievementID, 1, false, true);

                // 直接解锁按钮
                if (GUI.Button(new Rect(340, y + 2, 60, 22), "Unlock"))
                    UnlockAchievement(task.achievementID, true);
            }
        }

        GUI.EndScrollView();
    }

    /// <summary>
    /// 仅开发测试用：重置当前账号的 Steam Stats / Achievements。
    /// 不要在正式版本 UI 暴露。
    /// </summary>
    public bool ResetAllForTest(bool resetAchievementsToo = true)
    {
        if (!SteamManager.Initialized)
            return false;

        bool success = SteamUserStats.ResetAllStats(resetAchievementsToo);

        if (!success)
        {
            Debug.LogWarning("[AchievementSystem] ResetAllStats failed.");
            return false;
        }

        statsReady = false;
        dirty = false;
        storeInFlight = false;
        nextAutoStoreTime = 0f;

        foreach (AchievementTask task in achievementTasks)
        {
            if (task == null)
                continue;

            task.currentProgress = 0;
            task.unlocked = false;
        }

        RequestLatestStats();

        Debug.Log("[AchievementSystem] ResetAllStats called for testing.");

        return true;
    }
#endif
}