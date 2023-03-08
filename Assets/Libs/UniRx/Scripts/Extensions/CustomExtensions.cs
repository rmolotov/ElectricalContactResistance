using System;

namespace UniRx.Extensions
{
    public static class CustomExtensions
    {
        public static IObservable<Tuple<TSource, TSource>> PairWithPrevious<TSource>(this IObservable<TSource> source)
        {
            return source.Scan(
                Tuple.Create(default(TSource), default(TSource)),
                (acc, current) => Tuple.Create(acc.Item2, current));
        }
        
        public static IObservable<TResult> CombineWithPrevious<TSource,TResult>(
            this IObservable<TSource> source, Func<TSource, TSource, TResult> resultSelector)
        {
            return source.Scan(
                    Tuple.Create(default(TSource), default(TSource)),
                    (previous, current) => Tuple
                        .Create(previous.Item2, current))
                        .Select(t => resultSelector(t.Item1, t.Item2));
        }
    }
}