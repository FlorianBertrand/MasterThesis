using System;


namespace MT{
    /// <summary>
    /// ClassValue is a value that a class variable can take
    /// count is its occurence
    /// </summary>
    public class ClassValue
    {
        public string value;
        public int count;
        public ClassValue(string n)
        {
            value = n;
            count = 1;
        }
        public void AddOcc()
        {
            count++;
        }
        public string GetVal()
        {
            return value;
        }
        public int GetOcc()
        {
            return count;
        }
        public void increment()
        {
            count++;
        }
        public bool boolean()
        {
            string[] flagFalse = new string[] { "F", "false", "f", "False", "0" };
            string[] flagTrue = new string[] { "T", "true", "t", "True", "1" };

            foreach (string s in flagFalse)
            {
                if (value == s)
                {
                    return true;
                }
            }
            foreach (string s in flagTrue)
            {
                if (value == s)
                {
                    return true;
                }
            }
            return false;
        }

        public bool isFalse()
        {
            string[] flagFalse = new string[] { "F", "false", "f", "False", "0" };
            foreach (string s in flagFalse)
            {
                if (value == s)
                {
                    return true;
                }
            }
            return false;

        }
    }
}
