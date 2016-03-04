using Xunit;
using System.Collections.Generic;
using System;
using System.Data;
using System.Data.SqlClient;

namespace HairSalon
{
  public class StylistTest : IDisposable
  {
    public StylistTest()
    {
      DBConfiguration.ConnectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=hair_salon_test;Integrated Security=SSPI;";
    }

    [Fact]
    public void Test_StylistsEmptyAtFirst()
    {
      int result = Stylist.GetAll().Count;

      Assert.Equal(0, result);
    }

    [Fact]
    public void Test_ReturnsTrueForSameName()
    {
      Stylist firstStylist = new Stylist("Mary");
      Stylist secondStylist = new Stylist("Mary");

      Assert.Equal(firstStylist, secondStylist);
    }

    [Fact]
    public void Test_Save_SavesStylistToDatabase()
    {
      Stylist testStylist = new Stylist("Mary");
      testStylist.Save();

      List<Stylist> result = Stylist.GetAll();
      List<Stylist> testList = new List<Stylist> {testStylist};

      Assert.Equal(result, testList);
    }

    [Fact]
    public void Test_Save_AssingsIdToStylistObject()
    {
      Stylist testStylist = new Stylist("Mary");
      testStylist.Save();

      Stylist savedStylist = Stylist.GetAll()[0];

      int result = savedStylist.GetId();
      int testId = testStylist.GetId();

      Assert.Equal(result, testId);
    }

    [Fact]
    public void Test_Find_FindsStylistInDatebase()
    {
      Stylist testStylist = new Stylist("Mary");
      testStylist.Save();

      Stylist foundStylist = Stylist.Find(testStylist.GetId());

      Assert.Equal(testStylist, foundStylist);
    }

    public void Dispose()
    {
      Stylist.DeleteAll();
    }
  }
}
