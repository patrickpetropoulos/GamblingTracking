﻿using Server.Application.Managers;

namespace Server.WebApp
{
  public class ServerSystem
  {
    private Dictionary<String, Object> _services;

    private readonly static object _instanceLock = new Object();
    private readonly static object _serviceProviderLock = new Object();
    private static ServerSystem _instance;
    private static IServiceProvider _serviceProvider;

    protected ServerSystem()
    {

    }

    protected ServerSystem( IServiceProvider services, IConfiguration configuration )
    {
      _services = new Dictionary<String, Object>();
      //The trick is both files, the interface and the implemenetation are in the same namespace, I think
      //If using an SQL project, class cannot have the .SQL 
      //Sports Book Manager
      //var sportsBookManager = ActivatorUtilities.CreateInstance( services, Type.GetType( configuration["SportsBookManager:Instance"] ),
      //    new object[] { configuration } ) as ISportsBookManager;
      //SportsBookManager = sportsBookManager;
      //_services.Add( "SportsBookManager", sportsBookManager );

      var test = Type.GetType( configuration["Casino:Manager"] );
      //In appsettings, its full namespace of file + filename, then comma, then full name of project it is in
      var casinoManager = ActivatorUtilities.CreateInstance(services, Type.GetType(configuration["Casino:Manager"]), new object[] { configuration } ) as ICasinoManager; 
      _services.Add("CasinoManager", casinoManager);

      var casinoGameManager = ActivatorUtilities.CreateInstance( services, Type.GetType( configuration["CasinoGame:Manager"] ), new object[] { configuration } ) as ICasinoGameManager;
      _services.Add( "CasinoGameManager", casinoGameManager );

    }
    public static void CreateInstance()
    {
      lock( _instanceLock )
      {
        _instance = new ServerSystem();
      }
    }
    public static void CreateInstance( IServiceProvider services, IConfiguration configuration )
    {
      lock( _instanceLock )
      {
        _instance = new ServerSystem( services, configuration );
      }
    }
    public static ServerSystem Instance
    {
      get
      {
        lock( _instanceLock )
        {
          return _instance;
        }
      }
    }
    public T Get<T>( String name ) where T : class
    {
      if( !_services.ContainsKey( name ) )
        return (T)null;

      return (T)_services[name];
    }
    public void SetServiceProvider( IServiceProvider serviceProvider )
    {
      lock( _serviceProviderLock )
      {
        _serviceProvider = serviceProvider;
      }
    }
    public void SetLogger( ILoggerFactory loggerFactory )
    {

    }


  }
}
