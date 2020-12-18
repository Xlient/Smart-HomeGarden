
#include <CloudIoTCore.h>
#include "esp8266_mqtt.h"
#include <ArduinoJson.h>
#include <NTPClient.h>
#include <WiFiUdp.h>


// Get Network time
WiFiUDP ntpUDP;
NTPClient timeClient(ntpUDP, "pool.ntp.org");

////////////////////
int soilMoisture,
  soilMoisture2 ,
  lightResistance,
  temperature ,
  humidity;

/******** Prtotypes  *******/

String toJson();
void readSensorData();

//*******************************//

void setup()
{
  
  Serial.begin(115200);
  setupCloudIoT(); 
 timeClient.begin();
 timeClient.setTimeOffset(-5000);
}

static unsigned long lastMillis = 0;
void loop()
{
  if(!mqtt->loop())
  {
    mqtt->mqttConnect();
  }
  

  delay(10); 

if(Serial.available()  >= 5)
{
  readSensorData();
}

  if (millis() - lastMillis > 60000)
  {
    lastMillis = millis();
    publishTelemetry(toJson());
    
  } 
  delay(14400000);
}

String toJson()
{
 StaticJsonDocument<200> doc;

  doc["SoilMoisture"] =  soilMoisture;
  doc["SoilMoisture2"] =  soilMoisture2;
  doc["LightResistance"] =  lightResistance;
  doc["Temperature"] =  temperature;
  doc["Humidity"] = humidity;
  doc["Date"] = getDateNow();
  doc["Time"] = getTimeNow();

   String json;
   serializeJson(doc,json);
   return json;
}

void readSensorData(){
 
        soilMoisture = Serial.parseInt();
        soilMoisture2 =  Serial.parseInt();
        temperature =  Serial.parseInt();
        humidity =   Serial.parseInt();
        lightResistance = Serial.parseInt();
      
}

String getDateNow()
{
    timeClient.update();
    unsigned long epochTime = timeClient.getEpochTime();
    struct tm *ptm = gmtime ((time_t *)&epochTime);
    int currentDay = ptm->tm_mday;
    int currentMonth = ptm->tm_mon+1;
    int currentYear = ptm->tm_year+1900;
    String currentDate = String(currentMonth) + "-" + String(currentDay) + "-" + String(currentYear);
    return currentDate;
}

String getTimeNow()
{
    timeClient.update();
    return timeClient.getFormattedTime();
}
