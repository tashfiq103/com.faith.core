namespace com.faith.core
{
    using System.Collections;
    using UnityEngine;
    using UnityEngine.Events;

    public class AccountManager : MonoBehaviour
    {

        #region Custom Variables


        internal class CurrencyType
        {

            public UnityEvent<double, CoreEnums.ValueChangedState> OnBalanceChangedEvent;

            private string _nameOfCurrency;
            private CoreEnums.ValueChangedState _balanceState;
            private SavedData<double> _accountBalance;


            private bool isAnimationRunning;
            
            private double _targetedAccountBalance;

            #region Configuretion

            private void SetNewBalance(double amount)
            {
                _accountBalance.SetData(amount);
                OnBalanceChangedEvent?.Invoke(amount, _balanceState);
            }

            #endregion

            #region Public Callback

            public CurrencyType(string nameOfCurrency)
            {
                _nameOfCurrency         = nameOfCurrency;
                _accountBalance         = new SavedData<double>("AM_Currency_" + nameOfCurrency, 0);
                _balanceState           = CoreEnums.ValueChangedState.VALUE_UNCHANGED;
                _targetedAccountBalance  = _accountBalance.GetData();
            }

            public string GetCurrencyName()
            {
                return _nameOfCurrency;
            }

            public double GetCurrentBalance()
            {
                return _accountBalance.GetData();

            }

            public void SetNewTargetForAccountBalance(double amount)
            {
                if (amount == 0) _balanceState = CoreEnums.ValueChangedState.VALUE_UNCHANGED;
                else if (amount > 0) _balanceState = CoreEnums.ValueChangedState.VALUE_INCREASED;
                else _balanceState = CoreEnums.ValueChangedState.VALUE_DECREASED;
                _targetedAccountBalance += amount;
            }

            public bool IsAnimationRunning()
            {

                return isAnimationRunning;
            }

            public IEnumerator AnimationForChangingAccountBalance(float animationDuration = 0.1f, AnimationCurve animationCurve = null)
            {

                isAnimationRunning = true;

                float cycleLength = 0.0167f;
                float remainingTime = animationDuration;

                WaitForSeconds cycleDelay = new WaitForSeconds(cycleLength);

                while (remainingTime > 0)
                {

                    float progression = 1f - (remainingTime / animationDuration);

                    if (animationCurve != null) progression = animationCurve.Evaluate(progression);

                    double currentBalance = GetCurrentBalance();
                    double newBalance = currentBalance + ((_targetedAccountBalance - currentBalance) * progression);
                    SetNewBalance(newBalance);

                    yield return cycleDelay;
                    remainingTime -= cycleLength;
                }

                SetNewBalance(_targetedAccountBalance);
                isAnimationRunning = false;
            }

            #endregion
        }

        #endregion

        #region Public Variables

        public static AccountManager Instance;

#if UNITY_EDITOR

        public bool showAccountManagerSettings;

#endif

        public CoreEnums.InstanceBehaviour  instanceBehaviour;
        public AccountManagerSettings       accountManagerSettings;

        #endregion

        #region Private Variables

        private CurrencyType[] currencyTypes;

        #endregion

        #region Mono Behaviour

        private void Awake()
        {
            Initialization();
        }

        #endregion

        #region Configuretion

        private void Initialization()
        {

            switch (instanceBehaviour)
            {

                case CoreEnums.InstanceBehaviour.UseAsReference:

                    break;
                case CoreEnums.InstanceBehaviour.CashedAsInstance:

                    Instance = this;

                    break;
                case CoreEnums.InstanceBehaviour.Singleton:

                    if (Instance == null)
                    {
                        Instance = this;
                        DontDestroyOnLoad(gameObject);
                    }
                    else
                    {

                        Destroy(gameObject);
                    }

                    break;
            }

            int numberOfCurrency = accountManagerSettings.GetNumberOfAvailableCurrency();
            currencyTypes = new CurrencyType[numberOfCurrency];

            for (int i = 0; i < numberOfCurrency; i++)
            {
                currencyTypes[i] = new CurrencyType(accountManagerSettings.listOfCurrencyInfos[i].enumName);
            }
        }

        #endregion

        #region Public Callback

        public void AddBalance(double amount, AccountManagerCurrencyEnum currency = AccountManagerCurrencyEnum.DEFAULT)
        {

            int currencyIndex = (int)currency;
            currencyTypes[currencyIndex].SetNewTargetForAccountBalance(amount);

            if (!currencyTypes[currencyIndex].IsAnimationRunning())
            {

                StartCoroutine(currencyTypes[currencyIndex].AnimationForChangingAccountBalance(
                    accountManagerSettings.listOfCurrencyInfos[currencyIndex].currencyAnimationDuration,
                    accountManagerSettings.listOfCurrencyInfos[currencyIndex].animationCurve));
            }
        }

        public bool DeductBalance(double amount, AccountManagerCurrencyEnum currency = AccountManagerCurrencyEnum.DEFAULT)
        {

            int currencyIndex = (int)currency;
            double currentBalance = currencyTypes[currencyIndex].GetCurrentBalance();
            if ((currentBalance - amount) >= 0)
            {
                AddBalance(-amount, currency);
                return true;
            }

            CoreDebugger.Debug.LogError("Insufficient Balance!!");
            return false;

        }

        public void OnBalanceChangedEvent (UnityAction<double, CoreEnums.ValueChangedState> OnBalanceChange, AccountManagerCurrencyEnum currency = AccountManagerCurrencyEnum.DEFAULT)
        {
            int currencyIndex = (int)currency;
            currencyTypes[currencyIndex].OnBalanceChangedEvent.AddListener(OnBalanceChange);
            currencyTypes[currencyIndex].OnBalanceChangedEvent.Invoke(currencyTypes[currencyIndex].GetCurrentBalance(), CoreEnums.ValueChangedState.VALUE_UNCHANGED);
        }

        public string GetNameOfCurrency(AccountManagerCurrencyEnum currency = AccountManagerCurrencyEnum.DEFAULT)
        {

            return accountManagerSettings.listOfCurrencyInfos[(int)currency].currencyName;
        }

        public double GetCurrentBalance(AccountManagerCurrencyEnum currency = AccountManagerCurrencyEnum.DEFAULT)
        {

            return currencyTypes[(int)currency].GetCurrentBalance();
        }



        #endregion
    }

}

