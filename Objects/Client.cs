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
    private string Note;

    public Client(string name, int stylistId, DateTime appointmentDate, string notes, int id = 0)
    {
      Id = id;
      Name = name;
      StylistId = stylistId;
      AppointmentDate = appointmentDate;
      Note = notes;
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
    public string GetNote()
    {
      return Note;
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
    public void Setnotes(string notes)
    {
      Note = notes;
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
          bool notesEquality = this.GetNote() == newClient.GetNote();

          return (idEquality && nameEquality && stylistIdEquality && appointMentDateEquality && notesEquality);
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
        string clientNote = rdr.GetString(4);

        Client newClient = new Client(clientName, clientStylistId, clientAppointmentDate,  clientNote, clientId);
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

      SqlCommand cmd= new SqlCommand("INSERT INTO clients (name, stylist_id, appointment_date, note) OUTPUT INSERTED.id VALUES (@ClientName, @ClientStylistId, @ClientAppointmentDate, @ClientNote);", conn);

      SqlParameter nameParameter = new SqlParameter();
      nameParameter.ParameterName = "@ClientName";
      nameParameter.Value = this.GetName();

      SqlParameter stylistIdParameter = new SqlParameter();
      stylistIdParameter.ParameterName = "@ClientStylistId";
      stylistIdParameter.Value = this.GetStylistId();

      SqlParameter appointmentDateParameter = new SqlParameter();
      appointmentDateParameter.ParameterName = "@ClientAppointmentDate";
      appointmentDateParameter.Value = this.GetAppointmentDate();

      SqlParameter noteParameter = new SqlParameter();
      noteParameter.ParameterName = "@ClientNote";
      noteParameter.Value = this.GetNote();

      cmd.Parameters.Add(nameParameter);
      cmd.Parameters.Add(stylistIdParameter);
      cmd.Parameters.Add(appointmentDateParameter);
      cmd.Parameters.Add(noteParameter);

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

    public static Client Find(int id)
    {
      SqlConnection conn = DB.Connection();
      SqlDataReader rdr = null;
      conn.Open();

      SqlCommand cmd=  new SqlCommand("SELECT * FROM clients WHERE id = @ClientId;", conn);
      SqlParameter clientIdParameter = new SqlParameter();
      clientIdParameter.ParameterName = "@ClientId";
      clientIdParameter.Value = id.ToString();
      cmd.Parameters.Add(clientIdParameter);

      rdr = cmd.ExecuteReader();

      int foundClientId = 0;
      string foundClientName = null;
      int foundClientStylistId = 0;
      DateTime foundClientAppointmentDate = new DateTime();
      string foundClientNote = null;

      while(rdr.Read())
      {
        foundClientId = rdr.GetInt32(0);
        foundClientName = rdr.GetString(1);
        foundClientStylistId = rdr.GetInt32(2);
        foundClientAppointmentDate = rdr.GetDateTime(3);
        foundClientNote = rdr.GetString(4);
      }
      Client foundClient = new Client(foundClientName, foundClientStylistId, foundClientAppointmentDate, foundClientNote, foundClientId);

      if(rdr != null)
      {
        rdr.Close();
      }
      if(conn != null)
      {
        conn.Close();
      }
      return foundClient;
    }

    public void Update(string newName, DateTime newDate, string newNote)
    {
      SqlConnection conn = DB.Connection();
      SqlDataReader rdr;
      conn.Open();

      SqlCommand cmd= new SqlCommand("UPDATE clients SET name = @NewName, appointment_date = @NewDate, note = @NewNote OUTPUT INSERTED.name, INSERTED.appointment_date, INSERTED.note WHERE id = @ClientId;", conn);

      SqlParameter newNameParameter = new SqlParameter();
      newNameParameter.ParameterName = "@NewName";
      newNameParameter.Value = newName;
      cmd.Parameters.Add(newNameParameter);

      SqlParameter newDateParameter = new SqlParameter();
      newDateParameter.ParameterName = "@NewDate";
      newDateParameter.Value = newDate;
      cmd.Parameters.Add(newDateParameter);

      SqlParameter newNoteParameter = new SqlParameter();
      newNoteParameter.ParameterName = "@NewNote";
      newNoteParameter.Value = newNote;
      cmd.Parameters.Add(newNoteParameter);

      SqlParameter clientIdParameter = new SqlParameter();
      clientIdParameter.ParameterName = "@ClientId";
      clientIdParameter.Value = this.GetId();
      cmd.Parameters.Add(clientIdParameter);

      rdr = cmd.ExecuteReader();

      while(rdr.Read())
      {
        this.Name = rdr.GetString(0);
        this.AppointmentDate = rdr.GetDateTime(1);
        this.Note = rdr.GetString(2);
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

    public void Delete()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("DELETE FROM clients WHERE id = @ClientId;", conn);

      SqlParameter clientIdParameter = new SqlParameter();
      clientIdParameter.ParameterName = "@ClientId";
      clientIdParameter.Value = this.GetId();

      cmd.Parameters.Add(clientIdParameter);
      cmd.ExecuteNonQuery();

      if(conn != null)
      {
        conn.Close();
      }
    }

    public static Client Search(string name)
    {
      SqlConnection conn = DB.Connection();
      SqlDataReader rdr = null;
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM clients WHERE name = @ClientName;", conn);
      SqlParameter clientNameParameter = new SqlParameter();
      clientNameParameter.ParameterName = "@ClientName";
      clientNameParameter.Value = name;
      cmd.Parameters.Add(clientNameParameter);
      rdr = cmd.ExecuteReader();

      int foundClientId = 0;
      string foundClientName = null;
      int foundClientStylistId = 0;
      DateTime foundClientDate = new DateTime();
      string foundClientNote = null;

      while(rdr.Read())
      {
        foundClientId = rdr.GetInt32(0);
        foundClientName = rdr.GetString(1);
        foundClientStylistId = rdr.GetInt32(2);
        foundClientDate = rdr.GetDateTime(3);
        foundClientNote = rdr.GetString(4);
      }

      Client foundClient = new Client(foundClientName, foundClientStylistId, foundClientDate, foundClientNote, foundClientId);

      if(rdr != null)
      {
        rdr.Close();
      }
      if(conn != null)
      {
        conn.Close();
      }
      return foundClient;
    }

    public  Stylist GetStylist()
    {
      SqlConnection conn = DB.Connection();
      SqlDataReader rdr = null;
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM stylists WHERE id = @StylsitId;", conn);
      SqlParameter stylistIdParameter = new SqlParameter();
      stylistIdParameter.ParameterName = "@StylsitId";
      stylistIdParameter.Value = this.GetStylistId();
      cmd.Parameters.Add(stylistIdParameter);
      rdr = cmd.ExecuteReader();

      int foundStylistId = 0;
      string foundStylistName = null;

      while(rdr.Read())
      {
        foundStylistId = rdr.GetInt32(0);
        foundStylistName = rdr.GetString(1);
      }

      Stylist foundStylist = new Stylist(foundStylistName, foundStylistId);

      if(rdr != null)
      {
        rdr.Close();
      }
      if(conn != null)
      {
        conn.Close();
      }
      return foundStylist;
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
