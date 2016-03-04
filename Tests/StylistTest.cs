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
    public void Test_Find_FindsStylistInDatabase()
    {
      Stylist testStylist = new Stylist("Mary");
      testStylist.Save();

      Stylist foundStylist = Stylist.Find(testStylist.GetId());

      Assert.Equal(testStylist, foundStylist);
    }

    [Fact]
    public void Test_Update_UpdateStylsitInDatabase()
    {
      string name = "Mary";
      Stylist testStylist = new Stylist(name);
      testStylist.Save();
      string newName = "Annie";

      testStylist.Update(newName);

      string result = testStylist.GetName();

      Assert.Equal(newName, result);
    }

    [Fact]
    public void Test_GetClients_RetreiveAllClientsWithStylist()
    {
      Stylist testStylist = new Stylist("Mary");
      testStylist.Save();

      DateTime testDate = new DateTime(2016, 3, 14);

      Client firstClient = new Client("Joe", testStylist.GetId(), testDate);
      firstClient.Save();

      Client secondClient = new Client("Mike", testStylist.GetId(), testDate);
      secondClient.Save();

      List<Client> testClientList = new List<Client> {firstClient, secondClient};
      List<Client> resultClientList  = testStylist.GetClients();

      Assert.Equal(testClientList, resultClientList);
    }

    public void Dispose()
    {
      Stylist.DeleteAll();
    }
  }
}
