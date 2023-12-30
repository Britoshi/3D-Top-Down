namespace Game
{
    public abstract class Stat
    {
        private OnValueChangeFunction callbackFunctions; 
        protected void CallBack(float change)
        {
            if ((int)change == 0) return;
            //if (change > 0) BritoNode.print("so this is a positive:", change);
            //else
            callbackFunctions?.Invoke(this, change);
        }
        
        internal Stat ModifyAdditive(int value) => ModifyAdditive((float)value);
        internal Stat ModifyMultiplicative(int value) => ModifyMultiplicative((float)value);  
        public void AddCallBack(OnValueChangeFunction func) => callbackFunctions += func;
        
        public static explicit operator int(Stat self) => self.GetIntegerValue();
        public static explicit operator float(Stat self) => self.GetValue(); 

        #region abstract functions
        public delegate void OnValueChangeFunction(Stat stat, float changeAmount);
        public abstract float GetValue(); 
        public abstract int GetIntegerValue();  
        internal abstract Stat ModifyAdditive(float value);
        internal abstract Stat ModifyMultiplicative(float value);
        #endregion
        public abstract void SetValue(float value);

        #region public modification calls
        public Stat Add(int value) => ModifyAdditive(value);
        public Stat Add(float value) => ModifyAdditive(value);
        public Stat Subtract(int value) => ModifyAdditive(-value);
        public Stat Subtract(float value) => ModifyAdditive(-value);
        public Stat Multiply(int value) => ModifyMultiplicative(value);
        public Stat Multiply(float value) => ModifyMultiplicative(value); 
        public Stat Divide(int value) => Divide((float)value);
        public abstract Stat Divide(float value);
        public abstract uint GetMaximumValue(); 
        #endregion
        
        #region operator overload
        public static int operator +(int value, Stat self) => value + (int)self.GetValue();
        public static float operator +(float value, Stat self) => value + self.GetValue();
        public static int operator -(int value, Stat self) => (int)self.GetValue() - value;
        public static float operator -(float value, Stat self) => self.GetValue() - value;
        public static float operator *(float value, Stat self) => self.GetValue() * value;
        public static float operator *(int value, Stat self) => self.GetValue() * value;
        public static float operator /(float value, Stat self) => value / self.GetValue();
        public static float operator /(int value, Stat self) => value / self.GetValue();

        public static int operator +(Stat self, int value) => value + (int)self.GetValue();
        public static float operator +(Stat self, float value) => value + self.GetValue();
        public static int operator -(Stat self, int value) => (int)self.GetValue() - value;
        public static float operator -(Stat self, float value) => self.GetValue() - value;
        public static float operator *(Stat self, float value) => self.GetValue() * value;
        public static float operator *(Stat self,int value) => self.GetValue() * value;
        public static float operator /(Stat self, float value) => self.GetValue() / value;
        public static float operator /(Stat self, int value) => self.GetValue() / value;
         
        public static bool operator <=(Stat self, int value) => self.GetValue() <= value;
        public static bool operator <=(int value, Stat self) => value <= self.GetValue();
        public static bool operator <=(Stat self, float value) => self.GetValue() <= value;
        public static bool operator <=(float value, Stat self) => value <= self.GetValue();
         
        public static bool operator >=(Stat self, int value) => self.GetValue() >= value;
        public static bool operator >=(int value, Stat self) => value >= self.GetValue();
        public static bool operator >=(Stat self, float value) => self.GetValue() >= value;
        public static bool operator >=(float value, Stat self) => value >= self.GetValue();
         
        public static bool operator >(Stat self, int value) => self.GetValue() > value;
        public static bool operator >(int value, Stat self) => value > self.GetValue();
        public static bool operator >(Stat self, float value) => self.GetValue() > value;
        public static bool operator >(float value, Stat self) => value > self.GetValue();
         
        public static bool operator <(Stat self, int value) => self.GetValue() < value;
        public static bool operator <(int value, Stat self) => value < self.GetValue();
        public static bool operator <(Stat self, float value) => self.GetValue() < value;
        public static bool operator <(float value, Stat self) => value < self.GetValue();
         
        public static bool operator ==(Stat self, int value) => self.GetValue() == value;
        public static bool operator ==(int value, Stat self) => value == self.GetValue();
        public static bool operator ==(Stat self, float value) => self.GetValue() == value;
        public static bool operator ==(float value, Stat self) => value == self.GetValue();
         
        public static bool operator !=(Stat self, int value) => self.GetValue() != value;
        public static bool operator !=(int value, Stat self) => value != self.GetValue();
        public static bool operator !=(Stat self, float value) => self.GetValue() != value;
        public static bool operator !=(float value, Stat self) => value != self.GetValue();
        // Remember to also override the Equals and GetHashCode methods when you override the == and != operators.
        public override bool Equals(object obj)
        {
            if (obj is Stat statObj) 
                return GetValue() == statObj.GetValue(); 
            return false;
        }

        public override int GetHashCode() => GetValue().GetHashCode(); 

        #endregion
    }
}