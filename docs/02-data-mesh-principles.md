# Principles of a data mesh

In the sample solution we follow 4 basic principles of data mesh design. In this section of the docs we'll cover
each of them to give you a good understanding of the design principles.

## Data ownership is organized per domain

In order for teams to move faster and align better with the business we've made it so that teams are feature aligned
and connected to a business counterpart for that feature. This enables the teams to build better quality software at
higher speeds. This principle has been quite succesful for micro service architectures.

In a data mesh environment we apply the same principle by organizing data per domain just like you would organize
micro services per domain. The people who own the domain know what business rules apply and what the data related
to the domain means.

## Data is a product

When we want to get more out of our data it is important it to upgrade to a product state. Teams treat their micro
services as a product. They should do the same for their reporting, analytics, and machine learning data within their
domain.

In practice this means that teams can expose a set of REST endpoints for the operational side of their domain alongside
of a set of endpoints for their data. That data should be well-defined just like they do for their REST endpoints. And
finally, it should be of the best quality possible.

Keep in mind that teams can choose whatever tool they want to expose data for reporting, analytics, or machine learning.
The choice depends on what sort of data they're exposing, and what a likely use-case is for the data.

## Data is available everywhere with self-service in mind

Once you have proper organization and approach data as a product you'll find that you can more easily expose datasets
in your organization for everyone.

When you want to make data available everywhere it's best to do so with self-service in mind. We've learned from
building many micro services over the year that self-service operations is a huge advantage. Teams can move faster
and you'll run into less problems.

Self-service for data means that you'll want a solution that makes data discoverable. You'll also want to make sure that
access control is automated to a level where people can get access through an automated workflow with human approval.

## Data is governed wherever it is

Within a data mesh the data is owned by teams. From a governance perspective this means that you'll
have a new challenge. Many of the jobs that were performed by a governance body are no longer done by them. The teams
who own the data monitor its quality and description.

While teams control the data they still need to work together to enable the data mesh to work. If everyone uses
a different tool for discovery, description, or accesss control there will only chaos.

The governance body within your organization will need to come up with tools
that enable data discovery, description, and access control to data. By providing a global set of tools
for these aspects of you can harmonize the way teams work together from a data perspective.

It's also important to have some standardization in naming data sources, fields, etc. This makes data more accessible
for someone who wants to use it.

In the sample solution we've opted to document some standards for how the data is exposed by the different micro 
services. You can find these under [standards/01-introduction.md](standards/01-introduction.md).
