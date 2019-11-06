/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package com.mycompany.drone.simulator;

import com.fasterxml.jackson.annotation.JsonIgnoreProperties;
import com.fasterxml.jackson.annotation.JsonInclude;
import com.fasterxml.jackson.annotation.JsonProperty;

/**
 *
 * @author madsfalken
 */
@SuppressWarnings("unused")
@JsonIgnoreProperties(ignoreUnknown = true)
@JsonInclude(JsonInclude.Include.NON_NULL)
public class TokenHolder {

    @JsonProperty("access_token")
    private String accessToken;
    @JsonProperty("id_token")
    private String idToken;
    @JsonProperty("refresh_token")
    private String refreshToken;
    @JsonProperty("token_type")
    private String tokenType;
    @JsonProperty("expires_in")
    private long expiresIn;

    /**
     * Getter for the Auth0's access token.
     *
     * @return the access token or null if missing.
     */
    @JsonProperty("access_token")
    public String getAccessToken() {
        return accessToken;
    }

    /**
     * Getter for the Auth0's id token.
     *
     * @return the id token or null if missing.
     */
    @JsonProperty("id_token")
    public String getIdToken() {
        return idToken;
    }

    /**
     * Getter for the Auth0's refresh token.
     *
     * @return the refresh token or null if missing.
     */
    @JsonProperty("refresh_token")
    public String getRefreshToken() {
        return refreshToken;
    }

    /**
     * Getter for the token type.
     *
     * @return the token type or null if missing.
     */
    @JsonProperty("token_type")
    public String getTokenType() {
        return tokenType;
    }

    /**
     * Getter for the duration of this token in seconds since it was issued.
     *
     * @return the number of seconds in which this token will expire, since the time it was issued.
     */
    @JsonProperty("expires_in")
    public long getExpiresIn() {
        return expiresIn;
    }
}