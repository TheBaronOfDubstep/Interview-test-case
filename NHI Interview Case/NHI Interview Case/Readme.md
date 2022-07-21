## Refactoring

For the purposes of discussion possible refactoring, there's a few things that floats to the top of 
my list. First and foremost, most of the input sanitizing should be done withing a try-catch block with
proper handlings to ensure proper status messages are returned for each proper error, ref difference 
between 401, 204, and 404 HTTP status headers.

## Optimization

I chose to optimize the solution with a singleton backed repository, using the already established
practice of dependency injection. I don't as of now know any particular improvements that are feasible 
given the time constraints.

## Organizing the code

When I develop software, I tend to go with a folder hierarcy layout that have stuck with me 
since the introduction of WPF and MVVM. Why I choose this is a good question, I guess it is 
a matter of unconcious habits and personal taste.

## Future development if given more time.

I would attempt to add some sort of caching service for the basic user login objects, but I find that
this is more of a philosophical question whether to implement or not. Implementing this specific
cache will, in my humble opinion, be counter-productive as the full resolved user with details is
the data of interest. Caching the fully resolved user makes more sense as the paged calls to 
github are just lookup calls.

If I had more time, I would also look at implementing some form of GraphQL support so that we have a 
filtering method against Github. I would have tied this in with the user detail caching system, and 
ensure that the graphQL being sent to github would exclude the items already cached in the singleton.

## Swagger UI

I am more accustomed to using [Postman](https://postman.com), but Swagger turned out to fit my
needs just as well.

