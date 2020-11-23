Graviton SysLog Service
=======================
A simple but useful syslog server.

Graviton can collect logs from multiple sources and save the result into one or many files/destinations, depending on your setup. It can run as a standalone application or run as a service.

#### Installation
* Extract zip-file into a desired directory
* Type **Graviton.Service.exe install** from the command prompt to install Graviton as a service
* Start the service (**net start Graviton**)



#### Client log4net configuration example
```xml
    <!-- Simple example for a consuming RemoteSyslogAppender -->
    <!--*****************************************************-->
    <appender name="RemoteSyslogAppender" type="log4net.Appender.RemoteSyslogAppender">
         <layout type="log4net.Layout.PatternLayout"
              value="Example | %date{yyyy-MM-dd HH:mm:ss,fff} | %-5level | DEV | %P{log4net:HostName} | %-7property{pid} | %-5thread | %property{username} | %logger | %message | %exception | %newline" />
      <remoteAddress value="localhost" />
      <remotePort value="514" />
      <filter type="log4net.Filter.LevelRangeFilter">
          <levelMin value="DEBUG" />
      </filter>
    </appender>
    <!--
    The first value in the layout identifies the logger name. (in this case "Example")
    See loggerConfigurationSection in the App.config file. Here you can have as many 
    configurations as you need.
    -->
```
#### Example of loggerConfigurationSection in the Graviton app.config file
```xml
    <loggerConfigurationSection>
        <LoggerConfigurations>
              <logger loggerName="Example"
                      filename="C:\Temp\Example\Example.log"
                      maximumFileSize="10MB"
                      maxSizeRollBackups="10"
                      preserveLogFileNameExtension="true"
                      datePattern="yyyy-MM-dd'.log'"
                      logLevel="DEBUG"
                      />
        </LoggerConfigurations>
    </loggerConfigurationSection>
```
