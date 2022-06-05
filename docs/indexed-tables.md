see: [https://docs.microsoft.com/en-us/azure/architecture/patterns/index-table](https://docs.microsoft.com/en-us/azure/architecture/patterns/index-table)

# Indexed Tables

To optimize data fetching, endpoints are indexed by the following "rules"

| index | pointer | parameters |
| :----- | :------- | :---------- |
| `/{account}/{name}{version}`                                          | endpoint info (meta)                                      |                         |
| `/{account}/{name}{version}?page=<page_number>?meta=<include_data>`   | endpoint entires page<br/>Page size is set on `EndpointInfo` by endpoint manager)   |<ul><li> `page_number` number, **default = 1** : Entries page number (pre calculated on creation). if `page_number` <=0 the first page is returned. If `page_number` >= <max_endpoint_pages> then the last page is returned.</li><li> `meta` - false/true, **default = false**. specifies if pagination meta should return in reponse payload</li></ul> |