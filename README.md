# TransferoTeam Service

[![Build Status](https://dev.azure.com/alexruzenhack/dotnetknowledge.microservice.teamservice/_apis/build/status/alexruzenhack.dotnetknowledge.microservice.teamservice)](https://dev.azure.com/alexruzenhack/dotnetknowledge.microservice.teamservice/_build/latest?definitionId=1)

The first microservice sample from the Building Microservices with ASP.NET Core book.

*Table 3-1. Team service API*

| Resource | 	Method |	Description |
|----------|---------|--------------|
| /teams | 	GET |	Gets a list of all teams |
| /teams/{id} | GET |	Gets details for a single team |
| /teams/{id}/members |	GET |	Gets members of a team |
| /teams | POST |	Creates a new team |
| /teams/{id}/members |	POST |	Adds a member to a team |
| /teams/{id} |	PUT |	Updates team properties |
| /teams/{id}/members/{memberId} |	PUT |	Updates member properties |
| /teams/{id}/members/{memberId} |	DELETE |	Removes a member from the team |
| /teams/{id} |	DELETE |	Deletes an entire team |
