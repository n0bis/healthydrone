/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package com.mycompany.drone.simulator;

import lombok.AllArgsConstructor;
import lombok.Getter;
import lombok.NoArgsConstructor;
import lombok.Setter;

/**
 *
 * @author madsfalken
 */
@AllArgsConstructor
@NoArgsConstructor
@Getter
@Setter
public class Track {
    private String timestamp;
    private Location location;
    private int accuracy;
    private Location pilotLocation;
    private double altitudeMSL;
    private double altitudeAGL;
    private int altitudeAccuracy;
    private double heading;
    private double speed;
    private double battery;
    private int rss;
    private String source;
}