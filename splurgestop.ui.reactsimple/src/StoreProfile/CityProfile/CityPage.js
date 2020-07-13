import React, { Fragment, useEffect, useState } from 'react';
/** @jsx jsx */
import { css, jsx } from '@emotion/core';
import { Button } from 'react-bootstrap';
import 'bootstrap/dist/css/bootstrap.min.css';
import { Page } from './../../Components/Page';
import { updateCity } from './CityCommands';

export function CityPage({ match }) {
  const [city, setCity] = useState(null);
  const [citiesLoading, setCitiesLoading] = useState(true);
  const [isEditing, setEditing] = useState(false);

  useEffect(() => {
    const loadCity = async () => {
      const id = match.params.id;
      const url = 'https://localhost:44304/api/City/' + id;
      const response = await fetch(url);
      const data = await response.json();
      setCity(data);
      setCitiesLoading(false);
    };

    if (match.params.id) {
      const cityId = match.params.id;
      loadCity(cityId);
    }
  }, [match.params.id]);

  const editModeClick = () => {
    setEditing(!isEditing);
  };

  const handleSubmit = async () => {
    await updateCity({
      id: city.id.value,
      name: city.name,
    });
  };

  const changeHandler = (e) => {
    city.name = e.currentTarget.value;
    setCity(city);
  };

  return (
    <Page title={city?.name}>
      <Button onClick={editModeClick} className="float-right">
        Edit
      </Button>
      <div>
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
                    label="City name"
                    placeholder={city.name}
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
                  <h1>{city.name}</h1>
                </div>
              )}
            </div>
          </Fragment>
        )}
      </div>
    </Page>
  );
}
