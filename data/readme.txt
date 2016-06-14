Build for version ElasticSearch version 2.3.3

Install
1. download ElasticSearch https://download.elastic.co/elasticsearch/release/org/elasticsearch/distribution/zip/elasticsearch/2.3.3/elasticsearch-2.3.3.zip
2. Unpack and run it. To install it as service use: bin/service.bat install
3. Optional elasticsearch managment plugin HQ. To install it run: 
	plugin install royrusso/elasticsearch-HQ
	Default address: http://localhost:9200/_plugin/hq/
4. Build solution
5. API sample calls
	- Load sample data from file: http://localhost/api/ProductDetails/Import
	- Search for "aspi": http://localhost/api/ProductDetails/Search?query=aspi 
	- Get by package id: http://localhost/api/ProductDetails/104979
	- Delete by id:[Method Delete] http://localhost/api/ProductDetails/104979


