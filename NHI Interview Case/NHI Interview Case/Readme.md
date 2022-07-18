## Refactoring

I did chose to forego having a database backend to drive a proxy service. I honestly 
assumed it would have been outside the scope of this assignment to begin with.

## Optimization

As mentioned above, the best form for optimization when regarding the number of requests 
to the github API would have been a database backed proxy service. For best performance 
and lowest cost impact, a NOSQL database would have been sufficient. This would have meant
that we would have had an ever growing duplicate of the github user database, and I am
unsure of the benefits of chosing such an eccentric "optimization". 

## Organizing the code

When I develop software, I tend to go with a folder hierarcy layout that have stuck with me 
since the introduction of WPF and MVVM. Why I choose this is a good question, I guess it is 
a matter of unconcious habits and personal taste.

## Future development if given more time.

If being set to expand on this solution, my first priority would be to implement 
a proper proxy service. 

## Swagger UI

I am more accustomed to using Postman (https://postman.com), but Swagger turns out to fit my
needs just as well.

