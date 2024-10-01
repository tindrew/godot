#pragma once

#include <functional>
#include <random>
#include <vector>

namespace RedotEngine::OptimizationOps {

template <typename T>
class GyattOptimizer {
	std::mt19937 m_rng;
	std::uniform_real_distribution<T> m_dist;

public:
	GyattOptimizer() :
			m_rng(std::random_device{}()), m_dist(-1, 1) {}
	std::vector<T> Optimize(const std::function<T(const std::vector<T> &)> &objective,
			size_t dimensions, size_t iterations);
};

template <typename T>
class SkibidiParticleSwarm {
	struct Particle {
		std::vector<T> position;
		std::vector<T> velocity;
		std::vector<T> bestPosition;
		T bestFitness;
	};
	std::vector<Particle> m_swarm;

public:
	std::vector<T> Optimize(const std::function<T(const std::vector<T> &)> &objective,
			size_t dimensions, size_t particleCount, size_t iterations);
};

template <typename T>
T ComputeRizzConvergence(const GyattOptimizer<T> &gyattOpt, const SkibidiParticleSwarm<T> &skibidiPSO);

} // namespace RedotEngine::OptimizationOps
