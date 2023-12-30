
using UnityEngine;
namespace Game
{
    public abstract class Resource : Stat
    {   
        public override Stat Divide(float value) => ModifyMultiplicative(1f / value);  
        public override uint GetMaximumValue() => (uint)GetValue();  
    }

    [System.Serializable] 
    public class SignedResource : Resource
    {
        [SerializeField] private int _value;
        public SignedResource() : base() { }
        public SignedResource(int value) => _value = value;
        public override float GetValue() => _value;
        public override void SetValue(float value) => _value = (int)value;
        public override int GetIntegerValue()=> _value;
        internal override Stat ModifyAdditive(float value)
        {
            int prevValue = _value;
            _value += (int)value;
            CallBack(_value - prevValue);
            return this;
        }

        internal override Stat ModifyMultiplicative(float value)
        {
            int prevValue = _value;
            _value = (int)(float)(value * _value);
            CallBack(_value - prevValue);
            return this;
        }
    }

    [System.Serializable]
    public class UnSignedResource : Resource
    {
        [SerializeField] protected uint _value;
        public UnSignedResource() { }
        public UnSignedResource(uint value) => _value = value;

        public override float GetValue() => _value;
        public override void SetValue(float value) => _value = (uint)value;
        public override int GetIntegerValue()=> (int)_value;

        internal override Stat ModifyAdditive(float value)
        {
            int prevValue = (int)_value;
            int baseValue = (int)_value;
            baseValue += (int)value;
            if (baseValue < 0) _value = 0;
            else _value = (uint)baseValue; 
            CallBack(_value - prevValue);
            return this;
        }

        internal override Stat ModifyMultiplicative(float value)
        {
            int prevValue = (int)_value;
            int baseValue = (int)(float)(value * _value); 
            if (baseValue < 0) _value = 0;
            else _value = (uint)baseValue;
            CallBack(_value - prevValue);
            return this;
        }

    }
    public class FloatResource : Resource
    {
        [SerializeField] protected float _value;
        public FloatResource() { }
        public FloatResource(float value) => _value = value;

        public override float GetValue() => _value;
        public override void SetValue(float value) => _value = value;
        public override int GetIntegerValue() => (int)_value;

        internal override Stat ModifyAdditive(float value)
        {
            float prevValue = _value;
            float baseValue = _value;
            baseValue += value;
            if (baseValue < 0) _value = 0;
            else _value = (uint)baseValue;
            CallBack(_value - prevValue);
            return this;
        }

        internal override Stat ModifyMultiplicative(float value)
        {
            int prevValue = (int)_value;
            int baseValue = (int)(float)(value * _value);
            if (baseValue < 0) _value = 0;
            else _value = (uint)baseValue;
            CallBack(_value - prevValue);
            return this;
        }
    }
    [System.Serializable]
    public class CappedResource : FloatResource
    {
        public IntegerAttribute Maximum;

        public CappedResource(uint baseValue, IntegerAttribute maximum) : base(baseValue)
        {
            Maximum = maximum;
            maximum.AddCallBack(OnMaximumValueChanged);
        }
        public CappedResource(IntegerAttribute maximum) : base()
        {
            Maximum = maximum;
            maximum.AddCallBack(OnMaximumValueChanged);
        } 

        void OnMaximumValueChanged(Stat stat, float amount)
        {
            var prevVal = _value;
            var statVal = (int)stat.GetValue();
            if (statVal < prevVal)
            { 
                _value = (uint)statVal;
                CallBack(_value - prevVal);
            }
        }

        internal override Stat ModifyAdditive(float value)
        {
            float prevValue = _value;
            float baseValue = _value;
            baseValue += value; 
            if (baseValue > Maximum.GetValue()) _value = Maximum.GetValue(); 
            else if (baseValue < 0) _value = 0;
            else _value = baseValue;
            CallBack(_value - prevValue);
            return this;
        }

        internal override Stat ModifyMultiplicative(float value)
        {
            float prevValue = _value;
            float baseValue = value * _value;
            if (baseValue > Maximum.GetValue()) _value = Maximum.GetValue();
            else if (baseValue < 0) _value = 0;
            else _value = baseValue;
            CallBack(_value - prevValue);
            return this;
        }

        public override uint GetMaximumValue() => (uint)Maximum.GetValue();
    }

}