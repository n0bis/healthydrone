CREATE DATABASE healthydrone;

CREATE TABLE DockerContainers (
  Id text NOT NULL,  
  droneId text NOT NULL,  
  port integer NOT NULL
);

CREATE EXTENSION IF NOT EXISTS "uuid-ossp" WITH SCHEMA public;

CREATE TABLE Incidents (
  Id uuid PRIMARY KEY DEFAULT uuid_generate_v4(),  
  Date text NOT NULL,  
  DroneId uuid NOT NULL,
  OperationId uuid NOT NULL,
  Details text NOT NULL,
  Damage text NOT NULL,
  Actions text NOT NULL,
  Notes text NOT NULL
);

CREATE TYPE EType AS ENUM ('Hospital', 'Nursery');

CREATE TABLE LandingPoints (
  Id uuid PRIMARY KEY DEFAULT uuid_generate_v4(),
  latitude double precision NOT NULL,
  longitude double precision NOT NULL,
  callsign  text NOT NULL,
  description text NOT NULL,
  name text NOT NULL,
  address text NOT NULL,
  type EType NOT NULL
);