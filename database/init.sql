CREATE DATABASE healthydrone;

create table "DockerContainers"
(
  "Id"      text    not null primary key,
  port      integer not null,
  "droneId" text    not null
);

CREATE EXTENSION IF NOT EXISTS "uuid-ossp" WITH SCHEMA public;

create table "Landingpoints"
(
  id          uuid             not null primary key DEFAULT uuid_generate_v4(),
  latitude    double precision not null,
  longitude   double precision not null,
  callsign    varchar(10)      not null,
  description text             not null,
  name        text             not null,
  address     text             not null,
  type        smallint         not null
);

create table "Incidents"
(
  "Id"          uuid      not null primary key DEFAULT uuid_generate_v4(),
  "Date"        timestamp not null,
  "OperationId" uuid      not null,
  "DroneId"     uuid      not null,
  "Details"     text      not null,
  "Damage"      text      not null,
  "Actions"     text      not null,
  "Notes"       text      not null
);