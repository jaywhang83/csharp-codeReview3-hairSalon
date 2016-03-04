using System.Collections.Generic;
using Nancy;
using Nancy.ViewEngines.Razor;
using System;

namespace HairSalon
{
  public class HomeModule : NancyModule
  {
    public HomeModule()
    {
      Get["/"] = _ =>
      {
        List<Stylist> allStylists = Stylist.GetAll();
        return View["index.cshtml", allStylists];
      };
      Get["/clients"] = _ =>
      {
        List<Client> allClients = Client.GetAll();
        return View["clients.cshtml", allClients];
      };
      Get["/stylist/new"] = _ =>
      {
        return View["stylist_form.cshtml"];
      };
      Post["/stylist/new"] = _ =>
      {
        Stylist newStylist = new Stylist(Request.Form["stylist-name"]);
        newStylist.Save();
        return View["success.cshtml"];
      };
      Get["client/new"] = _ =>
      {
        List<Stylist> allStylists = Stylist.GetAll();
        return View["client_form.cshtml", allStylists];
      };
      Post["/client/new"] = _ =>
      {
        Client newClient = new Client(Request.Form["client-name"], Request.Form["stylist-id"], Request.Form["client-appointment-date"], Request.Form["client-note"]);
        newClient.Save();
        return View["success.cshtml"];
      };
      Post["/clients/delete"] = _ =>
      {
        Client.DeleteAll();
        return View["deleted.cshtml"];
      };
      Post["/stylists/delete"]= _ =>
      {
        Stylist.DeleteAll();
        return View["deleted.cshtml"];
      };
      Get["/stylist/{id}"] = Parameters =>
      {
        Dictionary<string, object> model = new Dictionary<string, object> ();
        var selectedStylist = Stylist.Find(Parameters.id);
        var stylistClients = selectedStylist.GetClients();
        model.Add("stylist", selectedStylist);
        model.Add("clients", stylistClients);
        return View["stylist.cshtml", model];
      };
      Post["/client/add"] = _ =>
      {
        Client newClient = new Client(Request.Form["client-name"], Request.Form["stylist-id"], Request.Form["client-appointment-date"], Request.Form["client-note"]);
        newClient.Save();
        return View["success.cshtml"];
      };
      Get["/stylist/edit/{id}"] = parameters =>
      {
        Stylist selectedStylist = Stylist.Find(parameters.id);
        return View["stylist_edit.cshtml", selectedStylist];
      };
      Patch["/stylist/edit/{id}"]= Parameters =>
      {
        Stylist selectedStylist = Stylist.Find(Parameters.id);
        selectedStylist.Update(Request.Form["stylist-name"]);
        return View["success.cshtml"];
      };
      Get["/stylist/delete/{id}"] = parameters =>
      {
        Stylist selectedStylist = Stylist.Find(parameters.id);
        return View["stylist_delete.cshtml", selectedStylist];
      };
      Delete["/stylist/delete/{id}"] = parameters =>
      {
        Stylist selectedStylist = Stylist.Find(parameters.id);
        selectedStylist.Delete();
        return View["deleted.cshtml"];
      };
      Get["/client/search"] = _ =>
      {
        return View["search.cshtml"];
      };
      Post["/client/search/result"] = _ =>
      {
        Dictionary<string, object> model = new Dictionary<string, object> ();
        List<Client> clientResults = new List<Client> {};
        string clientName = Request.Form["search-client"];
        var searchResult = Client.Search(clientName);
        foreach(var result in searchResult)
        {
          Console.WriteLine(result.GetName());
          var clientStylist = result.GetStylist();
          clientResults.Add(result);
          model.Add("stylists", clientStylist);
        }
        model.Add("clients", clientResults); 
        return View["search_results.cshtml", model];
      };
    }
  }
}
