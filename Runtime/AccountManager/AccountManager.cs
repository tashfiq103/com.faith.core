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

            public UnityEvent<double, BALANCE_STATE> OnBalanceChangedEvent;

            private string _nameOfCurrency;
            public BALANCE_STATE _balanceState;
            private PlayerPrefData<double> _accountBalance;


            private bool isAnimationRunning;
            
            private double targetedAccountBalance;

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
                _accountBalance         = new PlayerPrefData<double>("AM_Currency_" + nameOfCurrency, 0);
                _balanceState           = BALANCE_STATE.NONE;
                targetedAccountBalance  = _accountBalance.GetData();
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
                targetedAccountBalance += amount;
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
                    double newBalance = currentBalance + ((targetedAccountBalance - currentBalance) * progression);
                    SetNewBalance(newBalance);

                    yield return cycleDelay;
                    remainingTime -= cycleLength;
                }

                SetNewBalance(targetedAccountBalance);
                isAnimationRunning = false;
            }

            #endregion
        }

        #endregion

        #region Public Variables

        public static AccountManager Instance;

        public AccountManagerSettings accountManagerSettings;
        [Range(0.1f, 2f)]
        public float durationForAnimation = 0.1f;
        public AnimationCurve animationCurve = new AnimationCurve(new Keyframe[] { new Keyframe(0, 0), new Keyframe(1, 1) });

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

            Instance = this;

            int numberOfCurrency = (int)CURRENCY.NUMBER_OF_CURRENCY;
            currencyTypes = new CurrencyType[numberOfCurrency];

            for (int i = 0; i < numberOfCurrency; i++)
            {

                CURRENCY currency = (CURRENCY)i;
                currencyTypes[i] = new CurrencyType(currency.ToString());
            }
        }



        #endregion

        #region Public Callback

        public void AddBalance(double amount, CURRENCY currency = CURRENCY.DEFAULT)
        {

            int currencyIndex = (int)currency;
            currencyTypes[currencyIndex].SetNewTargetForAccountBalance(amount);

            if (!currencyTypes[currencyIndex].IsAnimationRunning())
            {

                StartCoroutine(currencyTypes[currencyIndex].AnimationForChangingAccountBalance(durationForAnimation, animationCurve));
            }
        }

        public bool DeductBalance(double amount, CURRENCY currency = CURRENCY.DEFAULT)
        {

            int currencyIndex = (int)currency;
            double currentBalance = currencyTypes[currencyIndex].GetCurrentBalance();
            if ((currentBalance - amount) >= 0)
            {
                AddBalance(-amount, currency);
                return true;
            }

            Debug.LogError("Insufficient Balance!!");
            return false;

        }

        public void OnBalanceChangedEvent (UnityAction<double,BALANCE_STATE> OnBalanceChange, CURRENCY currency = CURRENCY.DEFAULT)
        {
            currencyTypes[(int)currency].OnBalanceChangedEvent.AddListener(OnBalanceChange);
        }

        public void NotifyCurrentBalanceToAllRegisteredEvent() {

            foreach (CurrencyType currencyType in currencyTypes) {

                currencyType.OnBalanceChangedEvent.Invoke(currencyType.GetCurrentBalance(), currencyType._balanceState);
            }
        }

        public int GetNumberOfAvailableCurrency()
        {
            return (int)CURRENCY.NUMBER_OF_CURRENCY;
        }

        public string GetNameOfCurrency(CURRENCY currency = CURRENCY.DEFAULT)
        {

            return currencyTypes[(int)currency].GetCurrencyName();
        }

        public double GetCurrentBalance(CURRENCY currency = CURRENCY.DEFAULT)
        {

            return currencyTypes[(int)currency].GetCurrentBalance();
        }



        #endregion
    }

}

