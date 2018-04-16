namespace BAMCIS.PrestoClient.Model
{
    /// <summary>
    /// From java.util.function.Consumer.java
    /// </summary>
    public interface Consumer<T>
    {
        /// <summary>
        /// Performs this operation on the given argument.
        /// </summary>
        /// <param name="t"></param>
        void Accept(T t);

        /// <summary>
        /// Returns a composed Consumer that performs, in sequence, this operation followed by the after operation.
        /// </summary>
        /// <param name="after"></param>
        /// <returns></returns>
        Consumer<T> AndThen(Consumer<T> after);

    }
}
