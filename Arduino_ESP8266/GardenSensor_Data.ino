
#include <DHT.h>
#define DHTTYPE DHT11


DHT dht(6,DHTTYPE);
   const int soilSensor = A0;
   const int soilSensor2 = A1;
   const int waterPump = 3;
   const int waterPump2 = 9;
   const int photoresit = A3;

   int  waterTime = 10;
   int soilMoisture = 0;
   int soilMoisture2 = 0;
   int lightResistance = 0.0;
   int temperature = 0;
   int humidity  = 0;

   //******** Prtotypes  *******//

 void sendData();
 void waterPlants();
   
   //******** Prtotypes  *******//
   
void setup() {
  
 pinMode(soilSensor,INPUT);
 pinMode(soilSensor2,INPUT);
 pinMode(photoresit,INPUT);
 pinMode(waterPump,OUTPUT);
 pinMode(waterPump2,OUTPUT);


 Serial.begin(9600);
 Serial.setTimeout(60000);
 Serial1.begin(115200);  
 dht.begin();
}

void loop() {
 
 soilMoisture = analogRead(soilSensor);
 soilMoisture2 = analogRead(soilSensor2);
 lightResistance = analogRead(photoresit);
 temperature = dht.readTemperature(true);
 humidity = dht.readHumidity();

 
Serial.println("Printing...");
if (isnan(humidity) || isnan(temperature)) 
{
Serial.println("Failed to read from sensor, retrying...."); 
  temperature = dht.readTemperature(true);
  humidity = dht.readHumidity();
 
  }

   //report back to ESP8266 
 sendData();
 waterPlants();
 delay(21600000); // wait for another 6 hours
  
}

void sendData() 
{
    Serial1.println(soilMoisture);
    Serial1.println(",");
    
    Serial1.println(soilMoisture2);
   Serial1.println(",");
   
    Serial1.println(lightResistance);
    Serial1.println(",");
    
    Serial1.println(temperature);
    Serial1.println(",");
    
    Serial1.println(humidity);
    Serial1.println(",");
   Serial.println("report sent to esp8266");
  
  }

  void waterPlants()
  { 
    
    if (soilMoisture < 650) 
    {   
        Serial.println("Pumping on pump 1..");
       digitalWrite(waterPump,LOW);
             delay(waterTime*1000);
       digitalWrite(waterPump,HIGH); 
        Serial.println("Pumped stoped");
     }

     delay(3000);
    if(soilMoisture2 < 650){
      
     Serial.println("Pumping on pump 2");
    digitalWrite(waterPump2,LOW);
    delay(waterTime*1000);
    digitalWrite(waterPump2, HIGH);  
    Serial.println("Pumped stoped");
}
  }
    
    
