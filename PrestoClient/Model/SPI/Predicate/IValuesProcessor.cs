using System;

namespace BAMCIS.PrestoClient.Model.SPI.Predicate
{
    /// <summary>
    /// From com.facebook.presto.spi.predicate.ValuesProcessor.java
    /// </summary>
    public interface IValuesProcessor
    {
        T Transform<T>(Func<IRanges, T> rangesFunction, Func<IDiscreteValues, T> discreteValuesFunction, Func<IAllOrNone, T> allOrNoneFunction);

        void Consume(Consumer<IRanges> rangesConsumer, Consumer<IDiscreteValues> discreteValuesConsumer, Consumer<IAllOrNone> allOrNoneConsumer);
    }
}
