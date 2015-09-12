# Bussedly

The backend to the Bussed application, it provides a single clean interface to the internal
APIs used by actual real-time bus websites.

215 - route duid: 6350571126703260090

	curl -k -v -X GET -H "Content-Type: application/json" "http://localhost:50027/v1/buses/?left=-8.4008214622735977&right=-8.3930967003107071&top=51.88590865860202&bottom=51.877431503513762"
	curl -k -v -X GET -H "Content-Type: application/json" "http://localhost:50027/v1/stops/?left=-8.4008214622735977&right=-8.3930967003107071&top=51.88590865860202&bottom=51.877431503513762

   Request URL: http://www.buseireann.ie/inc/proto/stopPointTdi.php?latitude_north=186822972&latitude_south=186750972&longitude_east=-30202812&longitude_west=-30274812&_=1441998764206
   
   Request Method: GET
   
   Cookie: language=en_gb; cookie_from=Mahon Point (Johnson & Perrott Garage); __utma=93375374.1073668593.1441998755.1441998755.1441998755.1; __utmb=93375374.2.10.1441998755; __utmc=93375374; __utmz=93375374.1441998755.1.1.utmcsr=(direct)|utmccn=(direct)|utmcmd=(none); kvcd=1441998764960; km_ai=LIIO36M73Dzz0F8G7ouSxjL6y7E%3D; km_uq=; km_vs=1; km_lv=1441998765
   Accept: application/json, text/javascript, */*; q=0.01
{"stopPointTdi": {
    "bus_stop_14": {
        "duid": "6350786630982546655",
        "last_modification_timestamp": 1441984830062,
        "is_deleted": false,
        "type": 1,
        "stop_duid": {
            "structTag": 50464,
            "duid": "6350927368470662861",
            "foo": 0
        },
        "number": 1,
        "long_name": "Jacobs Island (Sanctuary Rd Outbound)",
        "latitude": 186776748,
        "longitude": -30221208,
        "bearing": 100,
        "code": "242911",
        "foo": 0
    },
    "bus_stop_15": {
        "duid": "6350786630982546645",
        "last_modification_timestamp": 1441984830062,
        "is_deleted": false,
        "type": 1,
        "stop_duid": {
            "structTag": 50464,
            "duid": "6350927368470662860",
            "foo": 0
        },
        "number": 1,
        "long_name": "Mahon Point (Jacobs Island)",
        "latitude": 186784452,
        "longitude": -30216492,
        "bearing": 10,
        "code": "242901",
        "foo": 0
    }
}}



   Request URL: http://www.buseireann.ie/inc/proto/stopPassageTdi.php?stop_point=6350786630982516945&_=1441998764207
   Request Method: GET
   Accept: application/json, text/javascript, */*; q=0.01
   {"stopPassageTdi": {
    "passage_0": {
        "duid": "-9223372006720603836",
        "last_modification_timestamp": 1441997327115,
        "is_deleted": false,
        "route_duid": {
            "structTag": 50466,
            "duid": "6350571126703260090",
            "foo": 0
        },
        "direction": 1,
        "trip_duid": {
            "structTag": 50471,
            "duid": "-9223372004533498486",
            "foo": 0
        },
        "stop_point_duid": {
            "structTag": 50465,
            "duid": "6350786630982516945",
            "foo": 0
        },
        "vehicle_duid": {
            "structTag": 50470,
            "duid": "6356266596935140167",
            "foo": 0
        },
        "arrival_data": {
            "scheduled_passage_time_utc": 1441989300,
            "actual_passage_time_utc": 1441988012,
            "service_mode": 1,
            "multilingual_direction_text": {
                "defaultValue": "Cloghroe",
                "foo": 0
            },
            "type": 1,
            "foo": 0
        },
        "congestion_level": 3,
        "accuracy_level": 3,
        "status": 3,
        "is_accessible": 0,
        "latitude": 186804464,
        "longitude": -30463452,
        "bearing": 115,
        "pattern_duid": {
            "structTag": 50472,
            "duid": "6349931210964875853",
            "foo": 0
        },
        "has_bike_rack": 0,
        "category": 254,
        "foo": 0
    },
    "passage_25": {
        "duid": "-9223372006720418140",
        "last_modification_timestamp": 1442040668295,
        "is_deleted": false,
        "route_duid": {
            "structTag": 50466,
            "duid": "6350571126703260090",
            "foo": 0
        },
        "direction": 1,
        "trip_duid": {
            "structTag": 50471,
            "duid": "-9223372004532912101",
            "foo": 0
        },
        "stop_point_duid": {
            "structTag": 50465,
            "duid": "6350786630982546655",
            "foo": 0
        },
        "vehicle_duid": {
            "structTag": 50470,
            "duid": "6356266596935139593",
            "foo": 0
        },
        "arrival_data": {
            "scheduled_passage_time_utc": 1442041140,
            "scheduled_passage_time": "07:59",
            "actual_passage_time_utc": 1442040757,
            "actual_passage_time": "07:52",
            "service_mode": 1,
            "multilingual_direction_text": {
                "defaultValue": "Grand Parade",
                "foo": 0
            },
            "type": 1,
            "foo": 0
        },
        "departure_data": {
            "scheduled_passage_time_utc": 1442041140,
            "scheduled_passage_time": "07:59",
            "actual_passage_time_utc": 1442040757,
            "actual_passage_time": "07:52",
            "service_mode": 1,
            "multilingual_direction_text": {
                "defaultValue": "Mahon Pt. Shopping",
                "foo": 0
            },
            "type": 1,
            "foo": 0
        },
        "congestion_level": 1,
        "accuracy_level": 3,
        "status": 1,
        "is_accessible": 0,
        "latitude": 186783583,
        "longitude": -30233758,
        "bearing": 160,
        "pattern_duid": {
            "structTag": 50472,
            "duid": "6349931210964875852",
            "foo": 0
        },
        "has_bike_rack": 0,
        "category": 254,
        "foo": 0
    },
    "foo": 0
}}




   Request URL: http://www.buseireann.ie/inc/proto/vehicleTdi.php?latitude_north=186980140&latitude_south=186591627&longitude_east=-30023785&longitude_west=-30459719&_=1441998764217
   Request Method: GET
   Accept: application/json, text/javascript, */*; q=0.01
   {"vehicleTdi": {
    "bus_0": {
        "duid": "6352185209772835079",
        "last_modification_timestamp": 1442000103372,
        "is_deleted": false,
        "category": 254,
        "trip_duid": {
            "structTag": 50471,
            "duid": "-9223372004533423271",
            "foo": 0
        },
        "geo_position_status": 1,
        "reference_time": 1442000103,
        "latitude": 186798848,
        "longitude": -30447922,
        "bearing": 113,
        "is_accessible": 0,
        "pattern_duid": {
            "structTag": 50472,
            "duid": "6349931210940432640",
            "foo": 0
        },
        "has_bike_rack": 0,
        "foo": 0
    },
    "foo": 0
}}





   Request URL: http://www.buseireann.ie/inc/proto/routes.php
   Request Method: GET
   Accept: application/javascript, */*; q=0.8
	var obj_routes = { 
	"routeTdi": { 
		"routes_0": {
			 "duid": "6350571126703260049",
			 "last_modification_timestamp": 1441984843198,
			 "is_deleted": false,
			 "short_name": "601",
			 "direction_extensions": {
				 "direction": 1,
				 "direction_name": "",
				 "foo": 0
				}, 
			"direction_extensions": {
				"direction": 2, 
				"direction_name": "", 
				"foo": 0
				}, 
			"number": 601,
			"category": 5,
			"foo": 0
		},
		"foo": 0
	};


		
   Request URL: http://www.buseireann.ie/inc/proto/bus_stop_points.php
   
   Accept: application/javascript, */*; q=0.8
   var obj_bus_stop_points = {
       "bus_stops": {
	       "bus_stop_0": {
		       "duid": "6350786630982833765",
			   "name": "Abbey Cross (Eastbound)",
			   "lat": 53.38777,
			   "lng": -8.68336,
			   "num": 530021
			}
			...
		}
	};



   Request URL: http://www.buseireann.ie/inc/proto/stopPassageTdi.php?trip=-9223372004532912101&_=1442039544595
   Accept: application/json, text/javascript, */*; q=0.01
   {"stopPassageTdi": {
    "passage_0": {
        "duid": "-9223372006720418152",
        "last_modification_timestamp": 1442039979849,
        "is_deleted": false,
        "route_duid": {
            "structTag": 50466,
            "duid": "6350571126703260090",
            "foo": 0
        },
        "direction": 1,
        "trip_duid": {
            "structTag": 50471,
            "duid": "-9223372004532912101",
            "foo": 0
        },
        "stop_point_duid": {
            "structTag": 50465,
            "duid": "6350786630982546535",
            "foo": 0
        },
        "vehicle_duid": {
            "structTag": 50470,
            "duid": "6356266596935139593",
            "foo": 0
        },
        "arrival_data": {
            "scheduled_passage_time_utc": 1442040300,
            "scheduled_passage_time": "07:45",
            "actual_passage_time_utc": 1442040392,
            "actual_passage_time": "07:46",
            "service_mode": 1,
            "multilingual_direction_text": {
                "defaultValue": "Grand Parade",
                "foo": 0
            },
            "type": 1,
            "foo": 0
        },
        "departure_data": {
            "scheduled_passage_time_utc": 1442040300,
            "scheduled_passage_time": "07:45",
            "actual_passage_time_utc": 1442040392,
            "actual_passage_time": "07:46",
            "service_mode": 1,
            "multilingual_direction_text": {
                "defaultValue": "Mahon Pt. Shopping",
                "foo": 0
            },
            "type": 1,
            "foo": 0
        },
        "congestion_level": 1,
        "accuracy_level": 3,
        "status": 1,
        "is_accessible": 0,
        "latitude": 186806279,
        "longitude": -30468281,
        "bearing": 137,
        "pattern_duid": {
            "structTag": 50472,
            "duid": "6349931210964875852",
            "foo": 0
        },
        "has_bike_rack": 0,
        "category": 254,
        "foo": 0
    },
    "passage_1": {
        "duid": "-9223372006720418151",
        "last_modification_timestamp": 1442039979849,
        "is_deleted": false,
        "route_duid": {
            "structTag": 50466,
            "duid": "6350571126703260090",
            "foo": 0
        },
        "direction": 1,
        "trip_duid": {
            "structTag": 50471,
            "duid": "-9223372004532912101",
            "foo": 0
        },
        "stop_point_duid": {
            "structTag": 50465,
            "duid": "6350786630982546545",
            "foo": 0
        },
        "vehicle_duid": {
            "structTag": 50470,
            "duid": "6356266596935139593",
            "foo": 0
        },
        "arrival_data": {
            "scheduled_passage_time_utc": 1442040360,
            "scheduled_passage_time": "07:46",
            "actual_passage_time_utc": 1442040452,
            "actual_passage_time": "07:47",
            "service_mode": 1,
            "multilingual_direction_text": {
                "defaultValue": "Grand Parade",
                "foo": 0
            },
            "type": 1,
            "foo": 0
        },
        "departure_data": {
            "scheduled_passage_time_utc": 1442040360,
            "scheduled_passage_time": "07:46",
            "actual_passage_time_utc": 1442040452,
            "actual_passage_time": "07:47",
            "service_mode": 1,
            "multilingual_direction_text": {
                "defaultValue": "Mahon Pt. Shopping",
                "foo": 0
            },
            "type": 1,
            "foo": 0
        },
        "congestion_level": 1,
        "accuracy_level": 3,
        "status": 1,
        "is_accessible": 0,
        "latitude": 186806279,
        "longitude": -30468281,
        "bearing": 137,
        "pattern_duid": {
            "structTag": 50472,
            "duid": "6349931210964875852",
            "foo": 0
        },
        "has_bike_rack": 0,
        "category": 254,
        "foo": 0
    },
    "foo": 0
}}