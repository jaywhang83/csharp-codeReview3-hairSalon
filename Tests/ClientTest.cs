using Xunit;
using System.Collections.Generic;
using System;
using System.Data;
using System.Data.SqlClient;

namespace HairSalon
{
  public class ClientTest : IDisposable
  {
    public ClientTest()
    {
      DBConfiguration.ConnectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=hair_salon_test;Integrated Security=SSPI;";
    }

    [Fact]
    public void Test_DatabaseEmptyAtFirst()
    {
      int result = Client.GetAll().Count;
      Console.WriteLine(result);

      Assert.Equal(0, result);
    }

    [Fact]
    public void Test_Equal_overrideTrueForSameName()
    {
      DateTime testDate = new DateTime(2016, 3, 14);
      Client firstClient = new Client("Joe", 1, testDate);
      Client secondClient = new Client("Joe", 1, testDate);

      Assert.Equal(firstClient, secondClient);
    }

    [Fact]
    public void Test_Save()
    {
      DateTime testDate = new DateTime(2016, 3, 14);
      Client testClient = new Client("Joe", 1, testDate);

      testClient.Save();

      List<Client> result = Client.GetAll();
      List<Client> testList = new List<Client>{testClient};

      Assert.Equal(result, testList);
    }

    [Fact]
    public void Test_Save_AssignsIdToObject()
    {
      DateTime testDate = new DateTime(2016, 3, 14);
      Client testClient = new Client("Joe", 1, testDate);

      testClient.Save();

      Client savedClient = Client.GetAll()[0];

      int result = savedClient.GetId();
      int testId = testClient.GetId();

      Assert.Equal(testId, result);

    }

    [Fact]
    public void Test_Find_FindsClientInDatabase()
    {
      DateTime testDate = new DateTime(2016, 3, 14);
      Client testClient = new Client("Joe", 1, testDate);
      testClient.Save();

      Client foundClient = Client.Find(testClient.GetId());

      Assert.Equal(testClient, foundClient);
    }

    [Fact]
    public void Test_Update_UpdateClientInDatabase()
    {
      string name = "Joe";
      DateTime testDate = new DateTime(2016, 3, 14);
      Client testClient = new Client(name, 1, testDate);
      testClient.Save();

      string newName = "Bob";
      DateTime newDate = new DateTime(2016, 4, 21);

      testClient.Update(newName, newDate);

      string result = testClient.GetName();
      DateTime resultDate = testClient.GetAppointmentDate();

      Assert.Equal(result, newName);
      Assert.Equal(resultDate, newDate); 
    }

    public void Dispose()
    {
      Client.DeleteAll();
    }
  }
}
