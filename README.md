# checkout-server

A simple WebAPI to handle cart interaction.
* POST /api/cart - inserts a drink to the cart
* PUT /api/cart - updates a drink in the cart
* DELETE /api/cart/drink/{name} - deletes a drink with the given name from the cart
* GET /api/cart/drink/{name} - retreivves a drink with the given name from the cart
* GET /api/cart - lists all drinks in the cart in alphabetical order

I added a few more controlles to make the API more simmetrical:
* DELETE /api/cart - clears the card
* POST /api/cart/drink/{name} - inserts a drink with the given name to the cart
* PUT api/cart/drink{name} - updates a drink with the given name in the cart
I would prefer to have the cart OR the drink API only but the task requires both at the moment

I have decided not to use any 3rd party libraries except from required by ASP.Net WebAPI itself
So the database is mocked with in-memory hashtable, but all methods made async to resemble EF
The IoC is mocked by using a custom simple controller activator with explicitely registered controllers
I would also replace method parameters checks with Conditions and model to entity mapping with AutoMapper

The Web API uses basic authentication with 3 predifined users:
* alex:password1    (YWxleDpwYXNzd29yZDE=)
* bob:password2     (Ym9iOnBhc3N3b3JkMg==)
* charlie:password3 (Y2hhcmxpZTpwYXNzd29yZDM=)
