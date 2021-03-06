
using System.Collections.Generic;

using ComLib.Lang.Core;


namespace ComLib.Lang.Types
{
    /// <summary>
    /// Used to store/wrap a value of either a basic type or instance of a class.
    /// </summary>
    public class LObject
    {
        /// <summary>
        /// The data type of this value.
        /// </summary>
        public LType Type;


        /// <summary>
        /// Gets the value of this object.
        /// </summary>
        /// <returns></returns>
        public virtual object GetValue()
        {
            return null;
        }


        /// <summary>
        /// Clones this value.
        /// </summary>
        /// <returns></returns>
        public virtual object Clone()
        {
            return this;
        }
    }
    

    



    /// <summary>
    /// LObjectType class that all types extend from.
    /// </summary>
    public class LObjectType : LType
    {
        /// <summary>
        /// Initialize
        /// </summary>
        public LObjectType()
        {
            this.Name = "object";
            this.FullName = "sys.object";
            this.TypeVal = TypeConstants.Any;
            this.IsSystemType = true;
        }


        /// <summary>
        /// Sets up the matrix of possible conversions from one type to another type.
        /// </summary>
        public override void SetupConversionMatrix()
        {
            this.AddConversionTo(TypeConstants.Array,     TypeConversionMode.NotSupported);
            this.AddConversionTo(TypeConstants.Bool,      TypeConversionMode.NotSupported);
            this.AddConversionTo(TypeConstants.Date,      TypeConversionMode.NotSupported);
            this.AddConversionTo(TypeConstants.Map,       TypeConversionMode.NotSupported);
            this.AddConversionTo(TypeConstants.Number,    TypeConversionMode.NotSupported);
            this.AddConversionTo(TypeConstants.Null,      TypeConversionMode.Supported);
            this.AddConversionTo(TypeConstants.String,    TypeConversionMode.NotSupported);
            this.AddConversionTo(TypeConstants.Time,      TypeConversionMode.NotSupported);
        }
    }



    /// <summary>
    /// Base type for all data types.
    /// </summary>
    public class LType
    {
        /// <summary>
        /// Map of conversions from one basic type to another basic type.
        /// </summary>
        protected  IDictionary<int, int> _basicConversions = new Dictionary<int,int>();


        /// <summary>
        /// Get the type name of this type.
        /// </summary>
        public string Name;


        /// <summary>
        /// Gets the full typename 
        /// </summary>
        public string FullName;


        /// <summary>
        /// Whether or not this is a system level datatype.
        /// </summary>
        public bool IsSystemType;


        /// <summary>
        /// The value of the type. a numeric value for basic types.
        /// </summary>
        public int TypeVal;


        /// <summary>
        /// Whether or not this is a primitive type ( eg.. number, bool, string, date, time ).
        /// </summary>
        /// <returns></returns>
        public bool IsPrimitiveType()
        {
            if (this.TypeVal == TypeConstants.Null) return true;
            return this.TypeVal >= TypeConstants.Bool
                && this.TypeVal <= TypeConstants.Time;
        }


        /// <summary>
        /// Whether or not this is a basic type e.g. bool, date.
        /// </summary>
        /// <returns></returns>
        public bool IsBuiltInType()
        {
            if (this.TypeVal == TypeConstants.Null) return true;
            return this.TypeVal >= TypeConstants.Bool 
                && this.TypeVal <= TypeConstants.Map;
        }


        /// <summary>
        /// Setup the conversion matrix from type to another.
        /// </summary>
        public virtual void SetupConversionMatrix()
        {
        }


        /// <summary>
        /// Adds a conversion flag that indicates if converting from this type to the supplied type is possible
        /// </summary>
        /// <param name="typeVal">See TypeConstants : The type value of the destination basic type to convert to</param>
        /// <param name="mode">See TypeConversionMode: Flag indicating mode of conversion</param>
        /// <returns></returns>
        public void AddConversionTo(int typeVal, int mode)
        {
            this._basicConversions[typeVal] = mode;
        }


        /// <summary>
        /// Sets up the matrix of possible conversions to all basic datatypes to the mode supplied.
        /// </summary>
        /// <param name="typeConversionMode">See TypeConversionMode</param>
        public void SetDefaultConversionMatrix(int typeConversionMode)
        {
            this.AddConversionTo(TypeConstants.Array,     typeConversionMode);
            this.AddConversionTo(TypeConstants.Bool,      typeConversionMode);
            this.AddConversionTo(TypeConstants.Date,      typeConversionMode);
            this.AddConversionTo(TypeConstants.Map,       typeConversionMode);
            this.AddConversionTo(TypeConstants.Number,    typeConversionMode);
            this.AddConversionTo(TypeConstants.Null,      typeConversionMode);
            this.AddConversionTo(TypeConstants.String,    typeConversionMode);
            this.AddConversionTo(TypeConstants.Time,      typeConversionMode);
        }
    }
}
