import React, { Fragment, useEffect, useState } from 'react';
/** @jsx jsx */
import { css, jsx } from '@emotion/core';
import { Table, Button } from 'react-bootstrap';
import 'bootstrap/dist/css/bootstrap.min.css';
import { Link } from 'react-router-dom';
import { Page } from './../../Components/Page';
import { deleteLocation } from './LocationCommands';

export function LocationList() {
  const [locations, setLocations] = useState(null);
  const [locationsLoading, setLocationsLoading] = useState(true);

  useEffect(() => {
    const loadLocations = async () => {
      const url = 'https://localhost:44304/api/Location';
      const response = await fetch(url);
      const data = await response.json();
      setLocations(data);
      setLocationsLoading(false);
    };

    loadLocations();
  }, []);

  const removeItem = (index) => {
    let data = locations.filter((_, i) => i !== index);
    setLocations(data);
  };

  const handleDelete = async (location) => {
    let index = location.findIndex((s) => s.id === location.id);
    removeItem(index);

    await deleteLocation({
      id: location.id,
    });
  };

  return (
    <Page title="Locations">
      <Link
        css={css`
          text-decoration: none;
        `}
        to={`Location/Add`}
      >
        {' '}
        <Button className="float-right">Add Location</Button>
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
          <title>Locations</title>
        </div>
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
          <Table bordered hover size="sm">
            <thead>
              <tr
                css={css`
                  background: burlywood;
                  text-align: center;
                  text-transform: uppercase;
                `}
              >
                <th>Location name</th>
                <th>Details</th>
                <th>Remove</th>
              </tr>
            </thead>
            {locations.map((location) => (
              <tbody key={location.id}>
                <tr>
                  <Fragment key={location.id}>
                    <td>
                      <Link
                        css={css`
                          text-decoration: none;
                        `}
                        to={`LocationInfo/${location.id}`}
                      >
                        {(location.cityName, location.countryName)}
                      </Link>
                    </td>
                    <td
                      css={css`
                        width: 5em;
                      `}
                    >
                      <Button
                        variant="info"
                        href={`LocationInfo/${location.id}`}
                      >
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
                          handleDelete(location);
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
