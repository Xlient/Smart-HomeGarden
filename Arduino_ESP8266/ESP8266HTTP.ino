
#include <ArduinoJson.h>
#include <ESP8266WiFi.h> 
#include <ESP8266HTTPClient.h>


//******** Variables *******//

const char* ssid = " Your SSID ";
const char* password = "superSecurePassword";

int soilMoisture,
  soilMoisture2 ,
  lightResistance,
  temperature ,
  humidity;

char* serverAddress = "http://192.168.0.199:45455/api";
HTTPClient client;


//******** Prtotypes  *******//

void toJson();
void readSensorData();
void sendDoc(StaticJsonDocument<200> doc);
//*******************************//


void setup() {
 pinMode(LED_BUILTIN,OUTPUT);
 Serial.begin(115200); 
 Serial.setTimeout(2000);

 WiFi.mode(WIFI_STA);
  WiFi.begin(ssid, password);          
  while (WiFi.status() != WL_CONNECTED) {
    delay(1000);
  }
}


void loop() {
while(Serial.available() < 5) //wait for incoming data
{
    digitalWrite(LED_BUILTIN,HIGH);
    delay(1500);
    digitalWrite(LED_BUILTIN,LOW);

continue;
}
readSensorData();
toJson();
delay(21600000); // wait another 6 hours
    
            
}

 void readSensorData()
{
       digitalWrite(LED_BUILTIN,HIGH);
        soilMoisture = Serial.parseInt();
        soilMoisture2 =  Serial.parseInt();
       lightResistance = Serial.parseInt();
        temperature =  Serial.parseInt();
        humidity =   Serial.parseInt();
      digitalWrite(LED_BUILTIN,LOW);

       
}


void toJson()
{
 
 StaticJsonDocument<200> doc;

  doc["SoilMoisture"] =  soilMoisture;
  doc["SoilMoisture2"] =  soilMoisture2;
  doc["LightResistance"] =  lightResistance;
  doc["Temperature"] =  temperature;
  doc["Humidity"] = humidity;
   sendDoc(doc);
}




void sendDoc(StaticJsonDocument<200> doc)
{
  client.begin(serverAddress);
  client.addHeader("Content-Type", "application/json"); 
  String json;
   serializeJsonPretty(doc,json);
   int StatusCode = client.POST(json); 

   if(StatusCode != HTTP_CODE_OK)
   {
    Serial.println(StatusCode);
    String response = client.getString();
    }
    client.end();
 }

  
