using System;
using System.Collections.Generic;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;

namespace LRUCache.Core.Tests
{
    public class LRUCacheTests
    {
        private readonly ITestOutputHelper output;

        public LRUCacheTests(ITestOutputHelper output)
            => this.output = output;

        [Fact]
        public void When_Initialized_CacheIsEmpty()
        {
            var cache = new LRUCache<int>(3);
            cache
                .DebugInfo()
                .Should()
                .Be("Empty");
            this.output.WriteLine(cache.DebugInfo());
        }

        [Fact]
        public void When_Adding_ItemsArePrepended()
        {
            var cache = new LRUCache<int>(3);
            cache.Add("1", 101);
            cache
                .DebugInfo()
                .Should()
                .Be("HEAD: Key 1 - TAIL: Key 1 > 1");

            cache.Add("3", 301);
            cache.Add("7", 701);
            cache
                .DebugInfo()
                .Should()
                .Be("HEAD: Key 7 - TAIL: Key 1 > 7, 3, 1");
        }

        [Fact]
        public void When_AddingBeyondCapacity_LastItemIsEvicted()
        {
            var cache = new LRUCache<int>(3);
            cache.Add("1", 101);
            cache.Add("3", 301);
            cache.Add("2", 201);
            cache.Add("7", 701);
            cache
                .DebugInfo()
                .Should()
                .Be("HEAD: Key 7 - TAIL: Key 3 > 7, 2, 3");
        }

        [Fact]
        public void When_GetItem_UnknownKeyThrowsException()
        {
            var cache = new LRUCache<int>(3);
            cache.Add("1", 101);
            cache.Add("3", 301);

            Action get = () => cache.Get("32");
            get
                .Should()
                .Throw<KeyNotFoundException>()
                .WithMessage("Key '32' not found in cache");
        }

        [Fact]
        public void When_GetItem_MovesToHead()
        {
            var cache = new LRUCache<int>(3);
            cache.Add("1", 101);
            cache.Add("3", 301);
            cache.Add("7", 701);

            cache.Get("3");
            cache
                .DebugInfo()
                .Should()
                .Be("HEAD: Key 3 - TAIL: Key 1 > 3, 7, 1");
        }

        [Fact]
        public void When_GetHeadItem_NoReorderingHappens()
        {
            var cache = new LRUCache<int>(3);
            cache.Add("1", 101);
            cache.Add("3", 301);
            cache.Add("7", 701);

            cache.Get("7");
            cache
                .DebugInfo()
                .Should()
                .Be("HEAD: Key 7 - TAIL: Key 1 > 7, 3, 1");
        }

        [Fact]
        public void When_GetTailItem_NoReorderingHappens()
        {
            var cache = new LRUCache<int>(3);
            cache.Add("1", 101);
            cache.Add("3", 301);
            cache.Add("7", 701);

            cache.Get("1");
            cache
               .DebugInfo()
               .Should()
               .Be("HEAD: Key 1 - TAIL: Key 3 > 1, 7, 3");
        }
    }
}
