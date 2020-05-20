﻿using System;
using Xunit;
using FluentAssertions;

namespace Minesweeper.Tests
{
    public class MineMapSpec
    {
        [Fact]
        public void Should_GenerateCountNearBombs()
        {
            // arrange
            MineItem[,] expect = new MineItem[5, 5]
            {
                { new MineItem { IsBomb = false, NearBombsCount = 0 }, new MineItem { IsBomb = false, NearBombsCount = 1 }, new MineItem { IsBomb = false, NearBombsCount = 1 }, new MineItem { IsBomb = false, NearBombsCount = 1 }, new MineItem { IsBomb = false, NearBombsCount = 0 } },
                { new MineItem { IsBomb = false, NearBombsCount = 0 }, new MineItem { IsBomb = false, NearBombsCount = 1 }, new MineItem { IsBomb = true, NearBombsCount = 1 }, new MineItem { IsBomb = false, NearBombsCount = 1 }, new MineItem { IsBomb = false, NearBombsCount = 0 } },
                { new MineItem { IsBomb = false, NearBombsCount = 1 }, new MineItem { IsBomb = false, NearBombsCount = 3 }, new MineItem { IsBomb = false, NearBombsCount = 3 }, new MineItem { IsBomb = false, NearBombsCount = 2 }, new MineItem { IsBomb = false, NearBombsCount = 0 } },
                { new MineItem { IsBomb = false, NearBombsCount = 1 }, new MineItem { IsBomb = true, NearBombsCount = 1 }, new MineItem { IsBomb = true, NearBombsCount = 1 }, new MineItem { IsBomb = false, NearBombsCount = 1 }, new MineItem { IsBomb = false, NearBombsCount = 0 } },
                { new MineItem { IsBomb = false, NearBombsCount = 1 }, new MineItem { IsBomb = false, NearBombsCount = 2 }, new MineItem { IsBomb = false, NearBombsCount = 2 }, new MineItem { IsBomb = false, NearBombsCount = 1 }, new MineItem { IsBomb = false, NearBombsCount = 0 } },
            };

            // act
            var mineMap = new MineMap(5, 5);
            mineMap.MineItems[1, 2].IsBomb = true;
            mineMap.MineItems[3, 1].IsBomb = true;
            mineMap.MineItems[3, 2].IsBomb = true;
            mineMap.GenerateCountNearBombs();

            // assert
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    mineMap.MineItems[i,j].ToString().Should().Be(expect[i, j].ToString(), $"[{i}, {j}]");
                }
            }
        }

        [Fact]
        public void Should_GenerateCountNearBombs2()
        {
            // arrange
            MineItem[,] expect = new MineItem[5, 5]
            {
                {
                    new MineItem { IsBomb = false, NearBombsCount = 0 },
                    new MineItem { IsBomb = false, NearBombsCount = 1 },
                    new MineItem { IsBomb = false, NearBombsCount = 2 },
                    new MineItem { IsBomb = true },
                    new MineItem { IsBomb = false, NearBombsCount = 1 }
                },
                {
                    new MineItem { IsBomb = false, NearBombsCount = 0 },
                    new MineItem { IsBomb = false, NearBombsCount = 1 },
                    new MineItem { IsBomb = true },
                    new MineItem { IsBomb = false, NearBombsCount = 2 },
                    new MineItem { IsBomb = false, NearBombsCount = 1 }
                },
                {
                    new MineItem { IsBomb = false, NearBombsCount = 1 },
                    new MineItem { IsBomb = false, NearBombsCount = 3 },
                    new MineItem { IsBomb = false, NearBombsCount = 3 },
                    new MineItem { IsBomb = false, NearBombsCount = 2 },
                    new MineItem { IsBomb = false, NearBombsCount = 0 }
                },
                {
                    new MineItem { IsBomb = false, NearBombsCount = 1 },
                    new MineItem { IsBomb = true },
                    new MineItem { IsBomb = true },
                    new MineItem { IsBomb = false, NearBombsCount = 1 },
                    new MineItem { IsBomb = false, NearBombsCount = 0 }
                },
                {
                    new MineItem { IsBomb = false, NearBombsCount = 1 },
                    new MineItem { IsBomb = false, NearBombsCount = 2 },
                    new MineItem { IsBomb = false, NearBombsCount = 2 },
                    new MineItem { IsBomb = false, NearBombsCount = 1 },
                    new MineItem { IsBomb = false, NearBombsCount = 0 }
                },
            };

            // act
            var mineMap = new MineMap(5, 5);
            mineMap.MineItems[0, 3].IsBomb = true;
            mineMap.MineItems[1, 2].IsBomb = true;
            mineMap.MineItems[3, 1].IsBomb = true;
            mineMap.MineItems[3, 2].IsBomb = true;

            mineMap.GenerateCountNearBombs();

            // assert
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    mineMap.MineItems[i, j].ToString().Should().Be(expect[i, j].ToString(), $"[{i}, {j}]");
                }
            }
        }

        [Fact]
        public void Should_GenerateBombs()
        {
            // arrange

            // act
            var mineMap = new MineMap(5, 5);
            mineMap.GenerateBombs(3);

            // assert
            int countBombs = 0;
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    if(mineMap.MineItems[i, j].IsBomb)
                    {
                        countBombs++;
                    }
                }
            }
            countBombs.Should().Be(3);
        }
    }
}