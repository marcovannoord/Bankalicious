# Bankalicious
bankalicious awesomelicious!

# Installation:
Enter these commands on a (freshly installed) Ubuntu Server 14.04
``` bash
sudo su
apt-get update
apt-get upgrade
apt-get install mono-complete
apt-get install apache2

#and if you haven't done so yet, install MySql with
apt-get install mysql-server

/etc/init.d/apache2 stop
apt-get install mono-apache-server2 libapache2-mod-mono libmono-i18n2.0-cil
apt-get install mono-apache-server4
chown -R -v ubuntu /var/www/ #edit the name ubuntu to your username that you use on the server
```

Now we can upload our asp.net application to the webserver. I've uploaded my application to `/var/www/html/`. My webapplication is called WebApplication5. If your application has a different name, you should change the references down below to your own application. 

We needed to let Apache know to use Mono when a request came in on port 80.  To do so, we edited the 000-default.conf file located in the /etc/apach2/sites-available directory using:
``` bash
nano /etc/apache2/sites-available/000-default.conf
```

We edit this file to look like this:
``` xml
<VirtualHost *:80>
	# The ServerName directive sets the request scheme, hostname and port that
	# the server uses to identify itself. This is used when creating
	# redirection URLs. In the context of virtual hosts, the ServerName
	# specifies what hostname must appear in the request's Host: header to
	# match this virtual host. For the default virtual host (this file) this
	# value is not decisive as it is used as a last resort host regardless.
	# However, you must set it for any further virtual host explicitly.
	ServerName testapi

	ServerAdmin webmaster@localhost
	DocumentRoot /var/www/html/WebApplication5/WebApplication5/ #pay attention that this directory exists. This path should point to the location of where your bin/ resides
  MonoServerPath testapi "/usr/bin/mod-mono-server4"
  MonoDebug testapi true
  MonoSetEnv testapi MONO_IOMAP=all
  MonoApplications testapi "/:/var/www/html/WebApplication5/WebApplication5/ #pay attention that this directory exists. This path should point to the location of where your bin/ resides

	# Available loglevels: trace8, ..., trace1, debug, info, notice, warn,
	# error, crit, alert, emerg.
	# It is also possible to configure the loglevel for particular
	# modules, e.g.
	#LogLevel info ssl:warn

	ErrorLog ${APACHE_LOG_DIR}/error.log
	CustomLog ${APACHE_LOG_DIR}/access.log combined
<Location "/">
    Allow from all
    Order allow,deny
    MonoSetServerAlias testapi
    SetHandler mono
    SetOutputFilter DEFLATE
    SetEnvIfNoCase Request_URI "\.(?:gif|jpe?g|png)$" no-gzip dont-vary
  </Location>
  <IfModule mod_deflate.c>
    AddOutputFilterByType DEFLATE text/html text/plain text/xml text/javascript
  </IfModule>
	# For most configuration files from conf-available/, which are
	# enabled or disabled at a global level, it is possible to
	# include a line for only one particular virtual host. For example the
	# following line enables the CGI configuration for this host only
	# after it has been globally disabled with "a2disconf".
	#Include conf-available/serve-cgi-bin.conf
</VirtualHost>

# vim: syntax=apache ts=4 sw=4 sts=4 sr noet
```

However, now we are running an old version of Mono, since the standard repository that Ubuntu uses, is outdated. To update mono, we do the following:

## Add Signing Key using 

``` wget "http://keyserver.ubuntu.com/pks/lookup?op=get&search=0x3FA7E0328081BFF6A14DA29AA6A19B38D3D831EF" -O out && sudo apt-key add out && rm out```

##Add Repository
```echo "deb http://download.mono-project.com/repo/debian wheezy main" | sudo tee /etc/apt/sources.list.d/mono-xamarin.list  ```
  
##Update repolist  
 `sudo apt-get update  `  
Now we can update our software using `sudo apt-get upgrade`. Finally, we bring mono to the latest version using `sudo apt-get install mono-complete`.

Then, we need to start apache:

`/etc/init.d/apache2 start`

You should be able to hit your application using `http://<public ip address>/`

#Things to consider when using mono on ubuntu
When using mono, some changes need to be made when running the webapp. 
Most importantly, keep in mind to change the connectionstring in web.config.

To be able to do some quick development, download WinSCP, and use the auto-syncing function to keep the remote (ubuntu) directory up to date with your project. 

#known issues:
You might encounter an `System.Security.SecurityException. Couldn't impersonate token.` error. To fix this, in web.config, edit this:
``` xml
  <system.codedom>
    <compilers>
      <compiler language="c#;cs;csharp" extension=".cs" type="Microsoft.CodeDom.Providers.DotNetCompilerPlatform.CSharpCodeProvider, Microsoft.CodeDom.Providers.DotNetCompilerPlatform, Version=1.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" warningLevel="4" compilerOptions="/langversion:6 /nowarn:1659;1699;1701" />
      <compiler language="vb;vbs;visualbasic;vbscript" extension=".vb" type="Microsoft.CodeDom.Providers.DotNetCompilerPlatform.VBCodeProvider, Microsoft.CodeDom.Providers.DotNetCompilerPlatform, Version=1.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" warningLevel="4" compilerOptions="/langversion:14 /nowarn:41008 /define:_MYTYPE=\&quot;Web\&quot; /optionInfer+" />
    </compilers>
  </system.codedom>
```
to
``` xml
  <system.codedom>
    <compilers>
    <!--
      <compiler language="c#;cs;csharp" extension=".cs" type="Microsoft.CodeDom.Providers.DotNetCompilerPlatform.CSharpCodeProvider, Microsoft.CodeDom.Providers.DotNetCompilerPlatform, Version=1.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" warningLevel="4" compilerOptions="/langversion:6 /nowarn:1659;1699;1701" />
      <compiler language="vb;vbs;visualbasic;vbscript" extension=".vb" type="Microsoft.CodeDom.Providers.DotNetCompilerPlatform.VBCodeProvider, Microsoft.CodeDom.Providers.DotNetCompilerPlatform, Version=1.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" warningLevel="4" compilerOptions="/langversion:14 /nowarn:41008 /define:_MYTYPE=\&quot;Web\&quot; /optionInfer+" />
    -->
    </compilers>
  </system.codedom>
```

#bronnen
Ik heb onder andere gebruik gemaakt van de tutorial op `http://www.bloomspire.com/blog/2015/3/9/how-to-host-aspnet-applications-on-linux-p3`, met aanpassing van de volgorde ivm breaking packages en de verouderde versie die daar wordt besproken.
