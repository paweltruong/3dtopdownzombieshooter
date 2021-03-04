using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class EnemyCleanerSystemTests
    {       
        /// <summary>
        /// When sorted array with enemyToCleanStatuses contains data (true->exploded, false->killed) should calculate proper count of exploded
        /// </summary>
        [TestCase(new bool[] { false, false, true }, 1)]
        [TestCase(new bool[] { true, true, true }, 3)]
        [TestCase(new bool[] { false, false, false }, 0)]
        public void Should_GetValidExplodedCount(bool[] enemiesToCleanStatus, int expectedExplodedCount)
        {
            //Arrange
            var sut = new EnemyCleanerSystem();

            //Act
            var explodedCount = sut.GetExplodedCount(enemiesToCleanStatus);

            //Assert
            Assert.AreEqual(expectedExplodedCount, explodedCount, "Invalid explodedEnemies to clean count");
        }

    }
}
