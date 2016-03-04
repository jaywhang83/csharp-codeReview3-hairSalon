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

    public void Dispose()
    {
      Stylist.DeleteAll();
    }
  }
}
