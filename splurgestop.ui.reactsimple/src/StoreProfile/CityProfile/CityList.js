import React, { Fragment, useEffect, useState } from 'react';
/** @jsx jsx */
import { css, jsx } from '@emotion/core';
import { Table, Button } from 'react-bootstrap';
import 'bootstrap/dist/css/bootstrap.min.css';
import { Link } from 'react-router-dom';
import { Page } from './../../Components/Page';
import { deleteCity } from './CityCommands';

export function CityList() {
  const [cities, setCities] = useState(null);
  const [citiesLoading, setCitiesLoading] = useState(true);

  useEffect(() => {
    const loadCities = async () => {
      const url = 'https://localhost:44304/api/City';
      const response = await fetch(url);
      const data = await response.json();
      setCities(data);
      setCitiesLoading(false);
    };

    loadCities();
  }, []);

  const removeItem = (index) => {
    let data = cities.filter((_, i) => i !== index);
    setCities(data);
  };

  const handleDelete = async (city) => {
    let index = cities.findIndex((s) => s.id === city.id);
    removeItem(index);

    await deleteCity({
      id: city.id,
    });
  };

  return (
    <Page title="Cities">
      <Link
        css={css`
          text-decoration: none;
        `}
        to={`City/Add`}
      >
        {' '}
        <Button className="float-right">Add City</Button>
      </Link>
      <div
        css={css`
          margin: 50px auto 20px auto;
          padding: 30px 12px;
        `}
      >
        <div
          css={css`
            display: flex;
            align-items: center;
            justify-content: space-between;
          `}
        >
          <title>Cities</title>
        </div>
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
          <Table bordered hover size="sm">
            <thead>
              <tr
                css={css`
                  background: burlywood;
                  text-align: center;
                  text-transform: uppercase;
                `}
              >
                <th>City name</th>
                <th>Details</th>
                <th>Remove</th>
              </tr>
            </thead>
            {cities.map((city) => (
              <tbody key={city.id}>
                <tr>
                  <Fragment key={city.id}>
                    <td>
                      <Link
                        css={css`
                          text-decoration: none;
                        `}
                        to={`CityInfo/${city.id}`}
                      >
                        {city.name}
                      </Link>
                    </td>
                    <td
                      css={css`
                        width: 5em;
                      `}
                    >
                      <Button variant="info" href={`CityInfo/${city.id}`}>
                        Show
                      </Button>
                    </td>
                    <td
                      css={css`
                        width: 5em;
                      `}
                    >
                      <Button
                        variant="danger"
                        onClick={() => {
                          handleDelete(city);
                        }}
                      >
                        Delete
                      </Button>
                    </td>
                  </Fragment>
                </tr>
              </tbody>
            ))}
          </Table>
        )}
      </div>
    </Page>
  );
}
