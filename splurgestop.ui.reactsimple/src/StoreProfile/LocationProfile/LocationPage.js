import React, { Fragment, useEffect, useState } from 'react';
/** @jsx jsx */
import { css, jsx } from '@emotion/core';
import { Button } from 'react-bootstrap';
import 'bootstrap/dist/css/bootstrap.min.css';
import { Page } from './../../Components/Page';
import { updateLocation } from './LocationCommands';

export function LocationPage({ match }) {
  const [location, setLocation] = useState(null);
  const [locationsLoading, setLocationsLoading] = useState(true);
  const [isEditing, setEditing] = useState(false);
  const [cities, setCities] = useState(null);
  const [citiesLoading, setCitiesLoading] = useState(true);
  const [countries, setCountries] = useState(null);
  const [countriesLoading, setCountriesLoading] = useState(true);

  useEffect(() => {
    const loadLocation = async () => {
      const id = match.params.id;
      const url = 'https://localhost:44304/api/Location/' + id;
      const response = await fetch(url);
      const data = await response.json();
      setLocation(data);
      setLocationsLoading(false);
    };

    const loadCities = async () => {
      const url = 'https://localhost:44304/api/City';
      const response = await fetch(url);
      const data = await response.json();
      setCities(data);
      setCitiesLoading(false);
    };

    const loadCountries = async () => {
      const url = 'https://localhost:44304/api/Country';
      const response = await fetch(url);
      const data = await response.json();
      setCountries(data);
      setCountriesLoading(false);
    };

    if (match.params.id) {
      const locationId = match.params.id;
      loadLocation(locationId);
      loadCities();
      loadCountries();
    }
  }, [match.params.id]);

  const editModeClick = () => {
    setEditing(!isEditing);
  };

  const handleCityChange = (event) => {
    setLocation({
      id: location.id.value,
      city: cities.find((x) => x.id === event.target.value),
      country: location.country.id,
    });
  };

  const handleCountryChange = (event) => {
    setLocation({
      id: location.id.value,
      city: location.city.id,
      country: countries.find((x) => x.id === event.target.value),
    });
  };

  const handleSubmit = async () => {
    console.log(location.city);
    await updateLocation({
      id: location.id,
      cityId: location.city.id,
      countryId: location.country.id,
    });
  };

  // const changeHandler = (e) => {
  //   location.name = e.currentTarget.value;
  //   setStore(store);
  // };

  return (
    <Page title={(location?.cityName, location?.countryName)}>
      <Button onClick={editModeClick} className="float-right">
        Edit
      </Button>
      <div>
        {locationsLoading ? (
          <div
            css={css`
              font-size: 16px;
              font-style: italic;
            `}
          >
            Loading...
          </div>
        ) : (
          <Fragment>
            <div
              css={css`
                margin-top: 5em;
              `}
            >
              {isEditing ? (
                <form onSubmit={handleSubmit}>
                  <div
                    css={css`
                      margin: 1em;
                    `}
                  >
                    {citiesLoading ? (
                      <div
                        css={css`
                          font-size: 16px;
                          font-style: italic;
                        `}
                      >
                        Loading...
                      </div>
                    ) : (
                      <div>
                        <label for="cities">City:</label>
                        <br />
                        <select
                          name="cityId"
                          id="cityId"
                          className="input"
                          type="text"
                          onChange={handleCityChange}
                        >
                          <option>Select city</option>
                          {cities.map((city) => (
                            <option value={city.id} key={city.id}>
                              {city.name}, {city.name}
                            </option>
                          ))}
                        </select>
                      </div>
                    )}
                  </div>
                  <div
                    css={css`
                      margin: 1em;
                    `}
                  >
                    {countriesLoading ? (
                      <div
                        css={css`
                          font-size: 16px;
                          font-style: italic;
                        `}
                      >
                        Loading...
                      </div>
                    ) : (
                      <div>
                        <label for="countries">Country:</label>
                        <br />
                        <select
                          name="countryId"
                          id="countryId"
                          className="input"
                          type="text"
                          onChange={handleCountryChange}
                        >
                          <option>Select country</option>
                          {cities.map((country) => (
                            <option value={country.id} key={country.id}>
                              {country.name}, {country.name}
                            </option>
                          ))}
                        </select>
                      </div>
                    )}
                  </div>
                  <input type="submit" value="Save" />
                </form>
              ) : (
                <div>
                  <h2>{location.city.name}</h2>
                  <h2>{location.country.name}</h2>
                </div>
              )}
            </div>
          </Fragment>
        )}
      </div>
    </Page>
  );
}
