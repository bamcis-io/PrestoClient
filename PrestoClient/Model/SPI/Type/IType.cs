using System.Collections.Generic;

namespace BAMCIS.PrestoClient.Model.SPI.Type
{
    /// <summary>
    /// From com.facebook.presto.spi.type.Type.java
    /// </summary>
    public interface IType
    {
        /// <summary>
        /// Gets the name of this type which must be case insensitive globally unique.
        /// The name of a user defined type must be a legal identifier in Presto.
        /// </summary>
        /// <returns></returns>
        TypeSignature GetTypeSignature();

        /// <summary>
        /// Returns the name of this type that should be displayed to end-users.
        /// </summary>
        /// <returns></returns>
        string GetDisplayName();

        /// <summary>
        /// True if the type supports equalTo and hash.
        /// </summary>
        /// <returns></returns>
        bool IsComparable();

        /// <summary>
        /// True if the type supports compareTo.
        /// </summary>
        /// <returns></returns>
        bool IsOrderable();

        /// <summary>
        /// Gets the Java class type used to represent this value on the stack during
        /// expression execution.This value is used to determine which method should
        /// be called on Cursor, RecordSet or RandomAccessBlock to fetch a value of
        /// this type.
        /// Currently, this must be boolean, long, double, or Slice.
        /// </summary>
        /// <returns></returns>
        System.Type GetJavaType();

        /// <summary>
        /// For parameterized types returns the list of parameters.
        /// </summary>
        /// <returns></returns>
        IEnumerable<IType> GetTypeParameter();
    }
}
