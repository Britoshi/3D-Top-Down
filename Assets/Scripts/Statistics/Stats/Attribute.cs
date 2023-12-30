using System;
using System.Collections.Generic; 

namespace Game
{
    public abstract class Attribute : Stat
    {
        public bool inheritBaseValue;
        protected float _multiplierValue = 1f;

        [NonSerialized] protected Attribute _parentAttribute;
        [NonSerialized] protected List<Attribute> _childrenAttribute;

        #region Parent Child 
        void AssignChild(Attribute child)
        {
            _childrenAttribute ??= new List<Attribute>();
            _childrenAttribute.Add(child);
            child.ModifyBaseValue(GetBaseValue(), true);
            child.ModifyMultiplicative(_multiplierValue);
        }  
        protected void AssignParent(Attribute parent)
        {
            _parentAttribute = parent;
            parent.AssignChild(this);
        }  
        #endregion

        #region Modification 
        protected void RecalculateValue()
        {
            var previousValue = GetValue();
            SetValue(GetBaseValue() * _multiplierValue);
            CallBack(GetValue() - previousValue);
        }  
        void ModifyBaseValue(float dynamicValue, bool inherited)
        {
            float value = (float)dynamicValue; 
            if (!inherited || inheritBaseValue) Modify(value);
            _childrenAttribute?.ForEach(childAttribute => childAttribute.ModifyBaseValue(value, true));

            void Modify(float value)
            {
                float modifiedBaseValue = GetBaseValue();
                modifiedBaseValue += value;
                SetBaseValue(modifiedBaseValue < 0 ? GetBaseValue() : modifiedBaseValue);
                RecalculateValue();
            }
        } 
        internal override Stat ModifyAdditive(float value)
        {
            ModifyBaseValue(value, false);
            return this;
        }

        void ApplyMultiplicativeModification(float value)
        {
            float multiplierValue = _multiplierValue;
            multiplierValue += value - 1;
            _multiplierValue = multiplierValue < 0 ? 0 : multiplierValue;
            RecalculateValue();
        }
        internal override Stat ModifyMultiplicative(float value)
        {
            ApplyMultiplicativeModification(value);
            _childrenAttribute?.ForEach(childAttribute => childAttribute.ApplyMultiplicativeModification(value));
            return this;
        }

        public override Stat Divide(float value) => ModifyMultiplicative(1 - value + 1);    
        internal void ModifyBaseValue(int value, bool inherited) => ModifyBaseValue((float)value, inherited); 
        #endregion

        internal abstract float GetBaseValue(); 
        internal abstract float GetMultiplier(); 
        protected abstract void SetBaseValue(float value);
    }

    [Serializable]
    public class IntegerAttribute : Attribute
    {
        private uint _value;
        private uint _baseValue = 0;   
        public IntegerAttribute(uint baseValue,  float multiplierValue)
        {
            _parentAttribute = null;
            _childrenAttribute = null;

            _baseValue = baseValue;
            _multiplierValue = multiplierValue; 

            RecalculateValue();
        }
        public IntegerAttribute(uint? baseValue = null, float? multiplierValue = null, Attribute parent = null, bool inheritBaseValue = true) : this(baseValue == null ? 0 : baseValue.Value, multiplierValue == null ? 1f : multiplierValue.Value)
        {
            if (parent != null)
            {
                this.inheritBaseValue = inheritBaseValue;
                AssignParent(parent);
            }
        }  
        protected override void SetBaseValue(float value) => _baseValue = (uint)value; 
        public override void SetValue(float value) => _value = (uint)value;
        public override uint GetMaximumValue() => (uint)GetValue(); 
        public override float GetValue() => _value;  
        public override int GetIntegerValue() => (int)_value;
        internal override float GetBaseValue() => _baseValue;  
        internal override float GetMultiplier() => _multiplierValue; 
    }

    [System.Serializable]
    public class FloatAttribute : Attribute
    {
        private float _value;
        private float _baseValue = 0f; 
        public FloatAttribute(float baseValue, float multiplierValue)
        {
            _parentAttribute = null;
            _childrenAttribute = null;

            _baseValue = baseValue;
            _multiplierValue = multiplierValue;

            RecalculateValue();
        }
        public FloatAttribute(float? baseValue = null, float? multiplierValue = null, Attribute parent = null, bool inheritBaseValue = true) : this(baseValue == null ? 0 : baseValue.Value, multiplierValue == null ? 1f : multiplierValue.Value)
        {
            if (parent != null)
            {
                this.inheritBaseValue = inheritBaseValue;
                AssignParent(parent);
            }
        }    

        protected override void SetBaseValue(float value) => _baseValue = value; 
        public override void SetValue(float value) => _value = value;
        public override uint GetMaximumValue() => (uint)GetValue();
        public override float GetValue() => _value;
        public override int GetIntegerValue() => (int)_value;
        internal override float GetBaseValue() => _baseValue;
        internal override float GetMultiplier() => _multiplierValue;
    }
}