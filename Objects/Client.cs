using System.Collections.Generic;
using System.Data.SqlClient;
using System;

namespace HairSalon
{
  public class Client
  {
    private int Id;
    private string Name;
    private int StylistId;
    private DateTime AppointmentDate;

    public Client(string name, int stylistId, DateTime appointmentDate, int id = 0)
    {
      Id = id;
      Name = name;
      StylistId = stylistId;
      AppointmentDate = appointmentDate;
    }

    public int GetId()
    {
      return Id;
    }
    public string GetName()
    {
      return Name;
    }
    public int GetStylistId()
    {
      return StylistId;
    }
    public DateTime GetAppointmentDate()
    {
      return AppointmentDate;
    }

    public void SetId(int id)
    {
      Id = id;
    }
    public void SetName(string name)
    {
      Name = name;
    }
    public void SetStylistId(int stylistId)
    {
      StylistId = stylistId;
    }

    public void SetStylistId(DateTime appointmentDate)
    {
      AppointmentDate = appointmentDate;
    }

    public override bool Equals(System.Object otherClient)
    {
      if(!(otherClient is Client))
      {
        return false;
      }
      else
      {
          Client newClient = (Client) otherClient;
          bool idEquality = this.GetId() == newClient.GetId();
          bool nameEquality = this.GetName() == newClient.GetName();
          bool stylistIdEquality = this.GetStylistId() == newClient.GetStylistId();
          bool appointMentDateEquality = this.GetAppointmentDate() == newClient.GetAppointmentDate();

          return (idEquality && nameEquality && stylistIdEquality && appointMentDateEquality);
      }
    }

    public static List<Client> GetAll()
    {
      List<Client> allClients = new List<Client>{};

      SqlConnection conn = DB.Connection();
      SqlDataReader rdr = null;
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM clients ORDER BY appointment_date DESC;", conn);
      rdr = cmd.ExecuteReader();

      while(rdr.Read())
      {
        int clientId = rdr.GetInt32(0);
        string clientName = rdr.GetString(1);
        int clientStylistId = rdr.GetInt32(2);
        DateTime clientAppointmentDate = rdr.GetDateTime(3);

        Client newClient = new Client(clientName, clientStylistId, clientAppointmentDate, clientId);
        allClients.Add(newClient);
      }

      if(rdr != null)
      {
        rdr.Close();
      }
      if(conn != null)
      {
        conn.Close();
      }
      return allClients;
    }

    public void Save()
    {
      SqlConnection conn = DB.Connection();
      SqlDataReader rdr;
      conn.Open();

      SqlCommand cmd= new SqlCommand("INSERT INTO clients (name, stylist_id, appointment_date) OUTPUT INSERTED.id VALUES (@ClientName, @ClientStylistId, @ClientAppointmentDate);", conn);

      SqlParameter nameParameter = new SqlParameter();
      nameParameter.ParameterName = "@ClientName";
      nameParameter.Value = this.GetName();

      SqlParameter stylistIdParameter = new SqlParameter();
      stylistIdParameter.ParameterName = "@ClientStylistId";
      stylistIdParameter.Value = this.GetStylistId();

      SqlParameter appointmentDateParameter = new SqlParameter();
      appointmentDateParameter.ParameterName = "@ClientAppointmentDate";
      appointmentDateParameter.Value = this.GetAppointmentDate();

      cmd.Parameters.Add(nameParameter);
      cmd.Parameters.Add(stylistIdParameter);
      cmd.Parameters.Add(appointmentDateParameter);

      rdr = cmd.ExecuteReader();

      while(rdr.Read())
      {
        this.Id = rdr.GetInt32(0);
      }
      if(rdr != null)
      {
        rdr.Close();
      }
      if(conn != null)
      {
        conn.Close();
      }
    }

    public static void DeleteAll()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();
      SqlCommand cmd = new SqlCommand("DELETE FROM clients;", conn);
      cmd.ExecuteNonQuery();
    }
  }
}
