using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace BAMCIS.PrestoClient.Model
{
    public class StringHelper
    {
        #region Private Fields

        private List<KeyValuePair<string, object>> Values;

        #endregion

        #region Public Properties

        public Type Type { get; }

        #endregion

        #region Constructors

        private StringHelper(Type type)
        {
            this.Type = type;
            this.Values = new List<KeyValuePair<string, object>>();
        }

        #endregion

        #region Public Methods

        public static StringHelper Build(object baseObject)
        {
            return new StringHelper(baseObject.GetType());
        }

        public StringHelper Add(string parameterName, object value)
        {
            this.Values.Add(new KeyValuePair<string, object>(parameterName, value));
            return this;
        }


        public override string ToString()
        {
            StringBuilder SB = new StringBuilder();
            SB.Append($"{this.Type.Name} {{");

            foreach (KeyValuePair<string, object> Item in this.Values)
            {
                object Value = Item.Value;

                if (typeof(IEnumerable).IsAssignableFrom(Value.GetType()))
                {
                    SB.Append($"{Item.Key}=[{String.Join(",", (IList)Value)}], ");
                }
                else
                {
                    SB.Append($"{Item.Key}={Value.ToString()}, ");
                }
            }

            SB.Length = SB.Length - 2; // Remove the last space and comma
            SB.Append("}");

            return SB.ToString();
        }
        #endregion
    }
}
