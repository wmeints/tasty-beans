# Kafka topic naming convention

This document describes the naming conventions for Kafka topics in the solution. We use these naming conventions to
provide a consistent and predictable naming standard. It makes it easier for developers to find the information they
need about a certain topic in Kafka.

## Naming structure

We follow this naming structure for the topics in the solution:

```text
<domain>.<classification>.<description>.<version>
```

### Domain

The domain is a well understood, permanent name of the system that data relates to. We prefer to use bounded contexts
for these and try to avoid product names as much as we can.

Examples of good domain names are:

* **catalog**: All events related to the catalog of products offered by recommend-coffee.
* **warehouse**: All events related to warehousing such as stock information and packaging.
* **customers**: All events related to customer management.
* **shipping**: All events related to shipping products to our customers.

### Classification

We have different types of events in our system that relate to what we're trying to achieve with the events.
The following classifications have been identified for the events in the system:

* **fct**: These are events that represent facts, such as CustomerRegistered, OrderShipped, ProductsOutOfStock.
  It's immutable data and represents things that happened in one of the systems that are part of the solution.
* **cdc**: Change data capture topics contain snapshots of data. These topics can be used to repopulate systems. 
  We're commonly using compacted topics to capture data of this kind.
* **cmd:** Command topics represent operations that occur in the system. This is typically implemented as
  request/response. Commands are named as a combination of a verb and a noun, e.g. UpdateCustomerDetails.
* **sys**: System topics are internal topics for a system. They're not to be consumed by other systems in the solution.

### Description

The description is the most important part of the topic name. Typically we use the name of the subject for the data.
This varies between different classifications.

For example, the description of a fact topic could be *customer-registered* while the name of a CDC topic typically
is named after the entity stored such as *customers*.

### Version

The version part of the name captures the major version of the data published on the topic. As data in the topic evolves
we may introduce breaking changes to the data. Whenever we introduce a breaking change we make sure to publish a new
version of the topic.

Each topic will start with version *0*.

## User topics

If you're planning on experimenting with a new topic, we recommend that you create a user topic. User topics
are identified as `<domain>.usr.<description>.<version>`.
