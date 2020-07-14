import React, { Fragment, useEffect, useState } from 'react';
/** @jsx jsx */
import { css, jsx } from '@emotion/core';
import 'bootstrap/dist/css/bootstrap.min.css';
import { Page } from './../../Components/Page';
import { addLocation } from './LocationCommands';
import { toast, ToastContainer } from 'react-toastify';
import 'react-toastify/dist/ReactToastify.min.css';

export function NewLocation() {
  const [location, setLocation] = useState(null);
  const [cities, setCities] = useState(null);
  const [citiesLoading, setCitiesLoading] = useState(true);
  const [countries, setCountries] = useState(null);
  const [countriesLoading, setCountriesLoading] = useState(true);

  useEffect(() => {
    const loadCountries = async () => {
      const url = 'https://localhost:44304/api/Country';
      const response = await fetch(url);
      const data = await response.json();
      setCountries(data);
      setCountriesLoading(false);
    };

    const loadCities = async () => {
      const url = 'https://localhost:44304/api/City';
      const response = await fetch(url);
      const data = await response.json();
      setCities(data);
      setCitiesLoading(false);
    };

    loadCountries();
    loadCities();
  }, []);

  const handleInputChange = (event) => {
    setLocation({
      id: null,
      cityId: location?.cityId,
      countryId: location?.countryId,
    });
  };

  const handleCityChange = (event) => {
    setLocation({
      id: null,
      cityId: event.target.value,
      countryId: location.country.id,
    });
  };

  const handleCountryChange = (event) => {
    setLocation({
      id: null,
      cityId: location.city.id,
      countryId: event.target.value,
    });
  };

  const notify = (info) => {
    toast.info(info);
  };

  const handleSubmit = async () => {
    let error = await addLocation({
      id: null,
      cityId: location?.cityId,
      countryId: location?.countryId,
    }).then(
      () => null,
      (location) => location,
    );

    if (error === null) {
      notify('Location added');
    } else {
      toast.error(
        <div>
          Location not added!
          <br />
          {error.message}
        </div>,
      );
    }
  };

  return (
    <Page title="Add a new location">
      <Fragment>
        <ToastContainer />
        <div>
          <form onSubmit={handleSubmit}>
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
              <div
                css={css`
                  margin: 1em;
                `}
              >
                <label for="cities">Select a city:</label>
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
                      {city.name}
                    </option>
                  ))}
                </select>
              </div>
            )}
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
              <div
                css={css`
                  margin: 1em;
                `}
              >
                <label for="countries">Select a country:</label>
                <select
                  name="countryId"
                  id="countryId"
                  className="input"
                  type="text"
                  onChange={handleCountryChange}
                >
                  <option>Select country</option>
                  {countries.map((country) => (
                    <option value={country.id} key={country.id}>
                      {country.name}
                    </option>
                  ))}
                </select>
              </div>
            )}
            <input type="submit" value="Save" />
          </form>
        </div>
      </Fragment>
    </Page>
  );
}
