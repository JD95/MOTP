using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public interface Traversable<T>{

	// Choose the next path with the smallest cost
	T getNext_Smallest();

	// Choose the next path with the largest cost
	T getNext_Largest();

	// Given two points on a graph, return the shortest path
	// from waypoint a to waypoint b
	List<T> shortestPath(T a, T b);
}