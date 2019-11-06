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
public class Location {
    private double longitude;
    private double latitude;
}
