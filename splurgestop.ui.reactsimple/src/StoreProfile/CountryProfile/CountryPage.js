import React, { Fragment, useEffect, useState } from 'react';
/** @jsx jsx */
import { css, jsx } from '@emotion/core';
import { Button } from 'react-bootstrap';
import 'bootstrap/dist/css/bootstrap.min.css';
import { Page } from './../../Components/Page';
import { updateCountry } from './CountryCommands';

export function CountryPage({ match }) {
  const [country, setCountry] = useState(null);
  const [countriesLoading, setCountriesLoading] = useState(true);
  const [isEditing, setEditing] = useState(false);

  useEffect(() => {
    const loadCountry = async () => {
      const id = match.params.id;
      const url = 'https://localhost:44304/api/Country/' + id;
      const response = await fetch(url);
      const data = await response.json();
      setCountry(data);
      setCountriesLoading(false);
    };

    if (match.params.id) {
      const countryId = match.params.id;
      loadCountry(countryId);
    }
  }, [match.params.id]);

  const editModeClick = () => {
    setEditing(!isEditing);
  };

  const handleSubmit = async () => {
    await updateCountry({
      id: country.id.value,
      name: country.name,
    });
  };

  const changeHandler = (e) => {
    country.name = e.currentTarget.value;
    setCountry(country);
  };

  return (
    <Page title={country?.name}>
      <Button onClick={editModeClick} className="float-right">
        Edit
      </Button>
      <div>
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
          <Fragment>
            <div
              css={css`
                margin-top: 5em;
              `}
            >
              {isEditing ? (
                <form onSubmit={handleSubmit}>
                  <input
                    type="text"
                    name="name"
                    label="Coutnry name"
                    placeholder={country.name}
                    onChange={changeHandler}
                  />
                  <div
                    css={css`
                      margin: 1em;
                    `}
                  ></div>
                  <input type="submit" value="Save" />
                </form>
              ) : (
                <div>
                  <h1>{country.name}</h1>
                </div>
              )}
            </div>
          </Fragment>
        )}
      </div>
    </Page>
  );
}
