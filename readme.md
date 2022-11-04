## Metar collector

A simple tool which get the latest metar data from Oslo Gardemoen 
and save it as XML.

XML output format:

```xml
<METAR>
    <Airport logtime="ISO-8601" icao="ENGM" 
    metarText="metar text" skyfritt="true/false"/>
</METAR>
```

This application use met.no weather API to collect metar data. See [documentation](https://api.met.no/weatherapi/tafmetar/1.0/documentation)
for more information.

### Settings

| Name | Description                    |
|------|--------------------------------|
| MetarServiceUrl| Base url to met.no weather API |
 | XmlOutputPath | Override name/path of xml file |
 | DefaultIcao | Default icao                   |


