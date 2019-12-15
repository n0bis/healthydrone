CREATE DATABASE healthydrone;

create table dockercontainers (
  id      text    not null primary key,
  port      integer not null,
  drone_id text    not null
);

CREATE EXTENSION IF NOT EXISTS "uuid-ossp" WITH SCHEMA public;

create table landingpoints
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

create table incidents
(
  id          uuid      not null primary key DEFAULT uuid_generate_v4(),
  date        timestamp not null,
  operation_id uuid      not null,
  drone_id     uuid      not null,
  details     text      not null,
  damage      text      not null,
  actions     text      not null,
  notes       text      not null
);