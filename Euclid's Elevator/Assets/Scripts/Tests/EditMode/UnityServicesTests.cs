using System.Collections;
using System.Collections.Generic;
using DeathFloor.UnityServices;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class UnityServicesTests
{
    [Test]
    public void GetDeltaTime_Returns_ANonZeroValue()
    {
        IUnityService service = new UnityService();

        float value = service.GetDeltaTime();

        Assert.AreNotEqual(value, 0);
    }
}
